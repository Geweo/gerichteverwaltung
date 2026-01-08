# PowerShell script to install all shadcn/ui components
# Run with: pwsh scripts/install-shadcn-components.ps1

$components = @(
  "accordion",
  "alert",
  "alert-dialog",
  "aspect-ratio",
  "avatar",
  "badge",
  "button",
  "calendar",
  "card",
  "carousel",
  "chart",
  "checkbox",
  "collapsible",
  "command",
  "context-menu",
  "dialog",
  "drawer",
  "dropdown-menu",
  "form",
  "hover-card",
  "input",
  "label",
  "menubar",
  "navigation-menu",
  "popover",
  "progress",
  "radio-group",
  "scroll-area",
  "select",
  "separator",
  "sheet",
  "skeleton",
  "slider",
  "sonner",
  "switch",
  "table",
  "tabs",
  "textarea",
  "toast",
  "toggle",
  "toggle-group",
  "tooltip"
)

Write-Host "Installing all shadcn/ui components..." -ForegroundColor Cyan
Write-Host "This may take a few minutes..." -ForegroundColor Yellow

foreach ($component in $components) {
  Write-Host "Installing $component..." -ForegroundColor Green
  npx shadcn-ui@latest add $component --yes --overwrite
}

Write-Host "✅ All components installed!" -ForegroundColor Green

