import { useState } from 'react';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import { Mic, MicOff, Loader2 } from 'lucide-react';
import { useSpeechToText } from '../hooks/useSpeechToText';
import { toast } from 'sonner';

interface PromptInputDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onConfirm: (prompt: string) => void;
  initialPrompt?: string;
}

/**
 * Dialog for entering a prompt with speech-to-text support.
 */
export function PromptInputDialog({
  open,
  onOpenChange,
  onConfirm,
  initialPrompt = '',
}: PromptInputDialogProps) {
  const [prompt, setPrompt] = useState(initialPrompt);
  const { isListening, transcript, error, startListening, stopListening, isSupported } =
    useSpeechToText({
      onResult: (text) => {
        setPrompt((prev) => {
          const newPrompt = prev ? `${prev} ${text}` : text;
          return newPrompt;
        });
      },
    });

  const handleConfirm = () => {
    if (!prompt.trim()) {
      toast.error('Bitte gib einen Prompt ein');
      return;
    }
    onConfirm(prompt.trim());
    onOpenChange(false);
    setPrompt('');
  };

  const handleCancel = () => {
    onOpenChange(false);
    setPrompt('');
    if (isListening) {
      stopListening();
    }
  };

  return (
    <Dialog open={open} onOpenChange={handleCancel}>
      <DialogContent className="max-w-2xl">
        <DialogHeader>
          <DialogTitle>Beschreibe dein Wunschgericht</DialogTitle>
          <DialogDescription>
            Gib eine Beschreibung ein oder nutze die Spracheingabe (benötigt Internetverbindung)
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-4 py-4">
          <div className="space-y-2">
            <Textarea
              placeholder="z.B. Italienisches Gericht, Pasta mit Gemüse, gesund und schnell..."
              className="resize-none min-h-[120px]"
              value={prompt}
              onChange={(e) => setPrompt(e.target.value)}
            />
            
            {/* Speech-to-Text Button */}
            {isSupported && (
              <div className="flex items-center gap-2 flex-wrap">
                <Button
                  type="button"
                  variant={isListening ? 'destructive' : 'outline'}
                  size="sm"
                  onClick={() => {
                    if (isListening) {
                      stopListening();
                    } else {
                      startListening();
                    }
                  }}
                >
                  {isListening ? (
                    <>
                      <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                      Aufnahme läuft...
                    </>
                  ) : (
                    <>
                      <Mic className="h-4 w-4 mr-2" />
                      Spracheingabe
                    </>
                  )}
                </Button>
                <span className="text-xs text-muted-foreground">
                  (benötigt Internet)
                </span>
                {isListening && (
                  <Button
                    type="button"
                    variant="ghost"
                    size="sm"
                    onClick={stopListening}
                  >
                    <MicOff className="h-4 w-4 mr-2" />
                    Stoppen
                  </Button>
                )}
                {transcript && !isListening && (
                  <p className="text-sm text-muted-foreground">
                    Erkannt: {transcript}
                  </p>
                )}
              </div>
            )}

            {error && (
              <div className="rounded-md bg-destructive/15 p-3">
                <p className="text-sm text-destructive font-medium">{error}</p>
                <p className="text-xs text-muted-foreground mt-1">
                  {error.includes('Netzwerkfehler') ? (
                    <>
                      <strong>Hinweis:</strong> Die Web Speech API benötigt normalerweise eine Internetverbindung, da sie einen Cloud-Service verwendet. 
                      Für eine lokale Spracherkennung ohne Internet müssten Sie eine alternative Lösung verwenden. 
                      Sie können jedoch weiterhin die Texteingabe nutzen.
                    </>
                  ) : (
                    <>
                      Tipp: Stellen Sie sicher, dass Ihr Mikrofon aktiviert ist und der Browser Zugriff darauf hat.
                    </>
                  )}
                </p>
              </div>
            )}

            {!isSupported && (
              <div className="rounded-md bg-muted p-3">
                <p className="text-sm font-medium mb-1">
                  Spracheingabe wird von Ihrem Browser nicht unterstützt.
                </p>
                <p className="text-xs text-muted-foreground">
                  Bitte verwenden Sie Chrome, Edge oder einen anderen Chromium-basierten Browser für die Spracheingabe.
                </p>
              </div>
            )}
          </div>

          <div className="flex justify-end gap-2">
            <Button variant="outline" onClick={handleCancel}>
              Abbrechen
            </Button>
            <Button onClick={handleConfirm} disabled={!prompt.trim()}>
              Bestätigen
            </Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
