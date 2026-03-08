# ClipboardManager

A macOS clipboard manager that keeps your last 5 copied items and lets you paste any of them on demand via a global hotkey.

## Data Flow

### Clipboard Monitoring (every 500ms)

```
MacClipboardMonitor polls NSPasteboard.changeCount
  └─ Count changed?
       └─ Read ALL types + raw bytes from pasteboard
            └─ Fire ClipboardChanged event
                 └─ HistoryManager.OnClipboardChanged()
                      ├─ Deduplicate (by content hash)
                      ├─ Prepend to list
                      ├─ Trim to 5 items
                      ├─ Persist via IHistoryRepository
                      └─ Fire HistoryChanged → HistoryViewModel updates
```

### Paste Flow (on hotkey press)

```
User presses Cmd+Shift+V
  └─ MacHotkeyService fires HotkeyTriggered
       └─ App shows HistoryWindow (Avalonia floating window)
            └─ User selects item (or presses 1–5)
                 └─ Write ALL types from RawData back to NSPasteboard
                      └─ Simulate Cmd+V via CGEvent P/Invoke
                           └─ Item pasted into active app
                                └─ HistoryWindow closes
```
xw
## Supported Content Types

| Copied Content | Types Stored |
|---|---|
| Plain text | `public.utf8-plain-text` |
| Text from browser | `public.utf8-plain-text`, `public.html`, `public.rtf` |
| Image | `public.png`, `public.tiff` |
| File from Finder | `public.file-url`, `public.utf8-plain-text` |
| Rich text (Word etc.) | `public.rtf`, `public.utf8-plain-text`, custom types |

## Storage Policy

| Type | Policy |
|---|---|
| Text / RTF / HTML / File | Always store full data |
| Images under 5MB | Store full data |
| Images over 5MB | Store thumbnail only |
| Total across all 5 items | Capped at ~20MB |

## Background Operation

The app runs as a macOS **Agent app** (`LSUIElement = true`) — no Dock icon, no app menu. It lives entirely in the menu bar.

**Tray menu:**
```
┌─────────────────────┐
│  Show History       │
│  ─────────────────  │
│  Launch at Login ✓  │
│  ─────────────────  │
│  Quit               │
└─────────────────────┘
```

On first launch, macOS will prompt for Pasteboard access. If denied, the app guides the user to System Settings → Privacy & Security → Pasteboard.

