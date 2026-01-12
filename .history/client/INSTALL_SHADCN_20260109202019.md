# ShadCN Komponenten Installation

## Problem
EPERM-Fehler beim Installieren der Komponenten - Berechtigungsproblem.

## Lösung

### Option 0: Permission-Problem beheben
Falls du einen EPERM-Fehler bekommst:

1. **Skript ausführen** (im `client` Ordner):
   ```powershell
   .\fix-pnpm-permission.ps1
   ```

2. **Oder manuell**:
   - Schließe alle Node-Prozesse (Dev-Server, etc.)
   - Lösche: `node_modules\.pnpm-workspace-state-v1.json`
   - Oder führe PowerShell als Administrator aus

### Option 1: Einzelne Installation (Empfohlen)
Installiere nur die benötigten Komponenten einzeln:

```bash
cd client
pnpm dlx shadcn@latest add button --yes
pnpm dlx shadcn@latest add card --yes
pnpm dlx shadcn@latest add input --yes
pnpm dlx shadcn@latest add textarea --yes
pnpm dlx shadcn@latest add badge --yes
pnpm dlx shadcn@latest add label --yes
pnpm dlx shadcn@latest add slider --yes
```

### Option 2: Als Administrator ausführen
1. Öffne PowerShell als Administrator
2. Navigiere zum Projekt: `cd C:\Users\keilm\source\repos\Ernährbär\client`
3. Führe aus: `pnpm shadcn:install-all`

### Option 3: Datei manuell löschen
1. Schließe alle Prozesse, die auf `node_modules` zugreifen (IDE, Terminal, etc.)
2. Lösche manuell: `client\node_modules\.pnpm-workspace-state-v1.json`
3. Führe dann erneut aus: `pnpm shadcn:install-all`

## Benötigte Komponenten für Rezepte-Seite
- button
- card
- input
- textarea
- badge
- label
- slider

## Nach der Installation
Die Komponenten werden in `client/src/components/ui/` installiert und können so importiert werden:

```typescript
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
```
