#!/usr/bin/env node

/**
 * Script to install all shadcn/ui components
 * Run with: node scripts/install-shadcn-components.js
 * or: pnpm shadcn:install-all
 */

import { execSync } from 'child_process';
import { fileURLToPath } from 'url';
import { dirname, join } from 'path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const projectRoot = join(__dirname, '..');

const components = [
  'accordion',
  'alert',
  'alert-dialog',
  'aspect-ratio',
  'avatar',
  'badge',
  'button',
  'calendar',
  'card',
  'carousel',
  'chart',
  'checkbox',
  'collapsible',
  'command',
  'context-menu',
  'dialog',
  'drawer',
  'dropdown-menu',
  'form',
  'hover-card',
  'input',
  'label',
  'menubar',
  'navigation-menu',
  'popover',
  'progress',
  'radio-group',
  'scroll-area',
  'select',
  'separator',
  'sheet',
  'skeleton',
  'slider',
  'sonner',
  'switch',
  'table',
  'tabs',
  'textarea',
  'toast',
  'toggle',
  'toggle-group',
  'tooltip',
];

console.log('🚀 Installing all shadcn/ui components...');
console.log('This may take a few minutes...\n');

let successCount = 0;
let errorCount = 0;

for (const component of components) {
  try {
    console.log(`📦 Installing ${component}...`);
    execSync(`npx shadcn@latest add ${component} --yes --overwrite`, {
      cwd: projectRoot,
      stdio: 'inherit',
    });
    successCount++;
    console.log(`✅ ${component} installed\n`);
  } catch (error) {
    errorCount++;
    console.error(`❌ Failed to install ${component}:`, error.message);
    console.log('Continuing with next component...\n');
  }
}

console.log('\n' + '='.repeat(50));
console.log(`✅ Installation complete!`);
console.log(`   Successfully installed: ${successCount} components`);
if (errorCount > 0) {
  console.log(`   Failed: ${errorCount} components`);
}
console.log('='.repeat(50));


