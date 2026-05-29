package main

import (
	"bytes"
	"fmt"
	"strconv"
	"strings"
	"time"

	"github.com/mabd-zadanko-2026/server/models"
)

const pdfLinesPerPage = 48

func buildMonthlyTasksPDF(username string, from, to time.Time, tasks []models.Task) []byte {
	lines := []string{
		"Raport zadan - ostatni miesiac",
		"Uzytkownik: " + username,
		"Zakres: " + from.Format("2006-01-02") + " - " + to.Format("2006-01-02"),
		"Wygenerowano: " + to.Format("2006-01-02 15:04"),
		"Liczba zadan: " + strconv.Itoa(len(tasks)),
		"",
	}

	if len(tasks) == 0 {
		lines = append(lines, "Brak zadan utworzonych w ostatnim miesiacu.")
	} else {
		for i, task := range tasks {
			status := "w trakcie"
			if task.IsDone {
				status = "skonczone"
			}

			lines = append(lines, wrapPDFLine(
				fmt.Sprintf("%d. [%s] %s", i+1, status, task.Title),
				92,
			)...)
			lines = append(lines, wrapPDFLine(
				fmt.Sprintf("   Kategoria: %s | Termin: %s | Utworzono: %s",
					task.Category,
					task.DueDate.Format("2006-01-02"),
					task.CreatedAt.Format("2006-01-02"),
				),
				92,
			)...)
			if task.Description != "" {
				lines = append(lines, wrapPDFLine("   Opis: "+task.Description, 92)...)
			}
			lines = append(lines, "")
		}
	}

	return renderSimplePDF(lines)
}

func renderSimplePDF(lines []string) []byte {
	pages := paginateLines(lines, pdfLinesPerPage)
	if len(pages) == 0 {
		pages = [][]string{{""}}
	}

	totalObjects := 3 + len(pages)*2
	offsets := make([]int, totalObjects+1)
	var out bytes.Buffer

	out.WriteString("%PDF-1.4\n")
	writePDFObject(&out, offsets, 1, "<< /Type /Catalog /Pages 2 0 R >>")

	var kids strings.Builder
	for i := range pages {
		pageObjectID := 4 + i*2
		kids.WriteString(fmt.Sprintf("%d 0 R ", pageObjectID))
	}
	writePDFObject(&out, offsets, 2, fmt.Sprintf(
		"<< /Type /Pages /Kids [%s] /Count %d >>",
		kids.String(),
		len(pages),
	))
	writePDFObject(&out, offsets, 3, "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>")

	for i, pageLines := range pages {
		pageObjectID := 4 + i*2
		contentObjectID := pageObjectID + 1
		content := buildPDFPageContent(pageLines, i+1, len(pages))

		writePDFObject(&out, offsets, pageObjectID, fmt.Sprintf(
			"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 3 0 R >> >> /Contents %d 0 R >>",
			contentObjectID,
		))
		writePDFStreamObject(&out, offsets, contentObjectID, content)
	}

	xrefOffset := out.Len()
	out.WriteString("xref\n")
	out.WriteString(fmt.Sprintf("0 %d\n", totalObjects+1))
	out.WriteString("0000000000 65535 f \n")
	for i := 1; i <= totalObjects; i++ {
		out.WriteString(fmt.Sprintf("%010d 00000 n \n", offsets[i]))
	}
	out.WriteString("trailer\n")
	out.WriteString(fmt.Sprintf("<< /Size %d /Root 1 0 R >>\n", totalObjects+1))
	out.WriteString("startxref\n")
	out.WriteString(fmt.Sprintf("%d\n", xrefOffset))
	out.WriteString("%%EOF")

	return out.Bytes()
}

func writePDFObject(out *bytes.Buffer, offsets []int, id int, body string) {
	offsets[id] = out.Len()
	out.WriteString(fmt.Sprintf("%d 0 obj\n%s\nendobj\n", id, body))
}

func writePDFStreamObject(out *bytes.Buffer, offsets []int, id int, content string) {
	offsets[id] = out.Len()
	out.WriteString(fmt.Sprintf("%d 0 obj\n<< /Length %d >>\nstream\n%s\nendstream\nendobj\n",
		id,
		len([]byte(content)),
		content,
	))
}

func buildPDFPageContent(lines []string, page, pageCount int) string {
	var content strings.Builder
	content.WriteString("BT\n")
	content.WriteString("/F1 11 Tf\n")
	content.WriteString("14 TL\n")
	content.WriteString("50 800 Td\n")
	for _, line := range lines {
		content.WriteString("(")
		content.WriteString(escapePDFText(sanitizePDFText(line)))
		content.WriteString(") Tj\n")
		content.WriteString("T*\n")
	}

	content.WriteString("T*\n")
	content.WriteString(fmt.Sprintf("(Strona %d/%d) Tj\n", page, pageCount))
	content.WriteString("ET")
	return content.String()
}

func paginateLines(lines []string, perPage int) [][]string {
	var pages [][]string
	for len(lines) > 0 {
		end := perPage
		if len(lines) < end {
			end = len(lines)
		}
		pages = append(pages, lines[:end])
		lines = lines[end:]
	}

	return pages
}

func wrapPDFLine(text string, maxLength int) []string {
	text = sanitizePDFText(text)
	words := strings.Fields(text)
	if len(words) == 0 {
		return []string{""}
	}

	var lines []string
	current := words[0]
	for _, word := range words[1:] {
		if len(current)+1+len(word) > maxLength {
			lines = append(lines, current)
			current = word
			continue
		}
		current += " " + word
	}
	lines = append(lines, current)

	return lines
}

func sanitizePDFText(text string) string {
	var builder strings.Builder
	for _, char := range text {
		if char == '\n' || char == '\r' || char == '\t' {
			builder.WriteByte(' ')
			continue
		}
		if replacement, ok := polishASCIIReplacement(char); ok {
			builder.WriteString(replacement)
			continue
		}
		if char < 32 || char > 126 {
			builder.WriteByte(' ')
			continue
		}
		builder.WriteRune(char)
	}

	return builder.String()
}

func polishASCIIReplacement(char rune) (string, bool) {
	switch char {
	case '\u0105', '\u0104':
		return "a", true
	case '\u0107', '\u0106':
		return "c", true
	case '\u0119', '\u0118':
		return "e", true
	case '\u0142', '\u0141':
		return "l", true
	case '\u0144', '\u0143':
		return "n", true
	case '\u00f3', '\u00d3':
		return "o", true
	case '\u015b', '\u015a':
		return "s", true
	case '\u017a', '\u0179', '\u017c', '\u017b':
		return "z", true
	default:
		return "", false
	}
}

func escapePDFText(text string) string {
	text = strings.ReplaceAll(text, `\`, `\\`)
	text = strings.ReplaceAll(text, "(", `\(`)
	text = strings.ReplaceAll(text, ")", `\)`)
	return text
}
