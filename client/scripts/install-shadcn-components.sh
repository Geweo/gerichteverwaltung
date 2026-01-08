#!/bin/bash

# Script to install all shadcn/ui components
# Run with: bash scripts/install-shadcn-components.sh

COMPONENTS=(
  "accordion"
  "alert"
  "alert-dialog"
  "aspect-ratio"
  "avatar"
  "badge"
  "button"
  "calendar"
  "card"
  "carousel"
  "chart"
  "checkbox"
  "collapsible"
  "command"
  "context-menu"
  "dialog"
  "drawer"
  "dropdown-menu"
  "form"
  "hover-card"
  "input"
  "label"
  "menubar"
  "navigation-menu"
  "popover"
  "progress"
  "radio-group"
  "scroll-area"
  "select"
  "separator"
  "sheet"
  "skeleton"
  "slider"
  "sonner"
  "switch"
  "table"
  "tabs"
  "textarea"
  "toast"
  "toggle"
  "toggle-group"
  "tooltip"
)

echo "Installing all shadcn/ui components..."
echo "This may take a few minutes..."

for component in "${COMPONENTS[@]}"; do
  echo "Installing $component..."
  npx shadcn-ui@latest add "$component" --yes --overwrite
done

echo "✅ All components installed!"

