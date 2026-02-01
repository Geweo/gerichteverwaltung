import { useState } from 'react';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { RecipeCreateForm } from './RecipeCreateForm';
import { RecipeAIGenerateForm } from './RecipeAIGenerateForm';

interface RecipeCreateDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

/**
 * Dialog for creating a new recipe.
 * Based on ERNAEHRBAR-Components.md - "Rezept-Erstellung"
 * 
 * Options:
 * - 📄 File Upload (PDF / PNG / mehrere)
 * - 🤖 KI-Gericht generieren
 * - ✏️ Manuell erstellen
 */
export function RecipeCreateDialog({ open, onOpenChange }: RecipeCreateDialogProps) {
  const [mode, setMode] = useState<'select' | 'upload' | 'ai' | 'manual'>('select');

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-3xl max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Neues Rezept erstellen</DialogTitle>
          <DialogDescription>
            Wähle eine Methode zum Erstellen eines neuen Rezepts
          </DialogDescription>
        </DialogHeader>

        {mode === 'select' && (
          <div className="grid grid-cols-1 gap-4 py-4">
            <Button
              variant="outline"
              className="h-24 flex-col"
              onClick={() => setMode('upload')}
            >
              <span className="text-2xl mb-2">📄</span>
              <span>File Upload</span>
              <span className="text-xs text-muted-foreground mt-1">
                PDF / PNG / mehrere Dateien
              </span>
            </Button>
            <Button
              variant="outline"
              className="h-24 flex-col"
              onClick={() => setMode('ai')}
            >
              <span className="text-2xl mb-2">🤖</span>
              <span>KI-Gericht generieren</span>
              <span className="text-xs text-muted-foreground mt-1">
                Mit KI-Unterstützung
              </span>
            </Button>
            <Button
              variant="outline"
              className="h-24 flex-col"
              onClick={() => setMode('manual')}
            >
              <span className="text-2xl mb-2">✏️</span>
              <span>Manuell erstellen</span>
              <span className="text-xs text-muted-foreground mt-1">
                Vollständige Kontrolle
              </span>
            </Button>
          </div>
        )}

        {mode !== 'select' && (
          <div className="py-4">
            <Button
              variant="ghost"
              onClick={() => setMode('select')}
              className="mb-4"
            >
              ← Zurück
            </Button>
            {mode === 'manual' && (
              <RecipeCreateForm
                onSuccess={() => onOpenChange(false)}
                onCancel={() => setMode('select')}
              />
            )}
            {mode === 'upload' && (
              <div className="text-center py-8">
                <p className="text-muted-foreground">
                  Upload-Funktion wird implementiert...
                </p>
              </div>
            )}
            {mode === 'ai' && (
              <RecipeAIGenerateForm
                onSuccess={() => onOpenChange(false)}
                onCancel={() => setMode('select')}
              />
            )}
          </div>
        )}
      </DialogContent>
    </Dialog>
  );
}
