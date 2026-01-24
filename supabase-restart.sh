#!/bin/bash
set -euo pipefail

# Outputs a formatted header with bold font and blue color
function h1() {
  local text="${*:-}"

  local BOLD BLUE RESET
  BOLD=$'\033[1m'
  BLUE=$'\033[34m'
  RESET=$'\033[0m'
  printf '\n%s%s%s\n' "${BOLD}${BLUE}" "-->  $text" "$RESET"
}

cd "$(dirname "$0")/ops/local/supabase"

h1 "Stopping supabase"
pnpm run stop || : # ignore error if supabase is not running

h1 "Starting supabase"
export MP_SEND_API_AUTH_ACCEPT_ANY=true
pnpm run start

h1 "Seeding supabase"
pnpm run seed || : # ignore error if seed script doesn't exist yet

h1 "Done"
