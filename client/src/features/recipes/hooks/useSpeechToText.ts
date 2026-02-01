import { useState, useRef, useEffect } from 'react';

interface UseSpeechToTextOptions {
  onResult?: (text: string) => void;
  language?: string;
}

/**
 * Hook for speech-to-text functionality using Web Speech API.
 */
export function useSpeechToText({ onResult, language = 'de-DE' }: UseSpeechToTextOptions = {}) {
  const [isListening, setIsListening] = useState(false);
  const [transcript, setTranscript] = useState('');
  const [error, setError] = useState<string | null>(null);
  const recognitionRef = useRef<SpeechRecognition | null>(null);

  useEffect(() => {
    // Check if browser supports Speech Recognition
    const SpeechRecognition = window.SpeechRecognition || (window as any).webkitSpeechRecognition;
    
    if (!SpeechRecognition) {
      setError('Spracherkennung wird von Ihrem Browser nicht unterstützt. Bitte verwenden Sie Chrome, Edge oder einen anderen Chromium-basierten Browser.');
      return;
    }

    // Create recognition instance
    const createRecognition = () => {
      try {
        const recognition = new SpeechRecognition();
        recognition.continuous = true;
        recognition.interimResults = true;
        recognition.lang = language;

        recognition.onstart = () => {
          setIsListening(true);
          setError(null);
        };

        recognition.onresult = (event: SpeechRecognitionEvent) => {
          let interimTranscript = '';
          let finalTranscript = '';

          for (let i = event.resultIndex; i < event.results.length; i++) {
            const transcript = event.results[i][0].transcript;
            if (event.results[i].isFinal) {
              finalTranscript += transcript + ' ';
            } else {
              interimTranscript += transcript;
            }
          }

          const fullTranscript = finalTranscript || interimTranscript;
          setTranscript(fullTranscript);
          
          if (finalTranscript && onResult) {
            onResult(finalTranscript.trim());
          }
        };

        recognition.onerror = (event: SpeechRecognitionErrorEvent) => {
          console.error('Speech recognition error:', event.error, event.message);
          setIsListening(false);
          
          switch (event.error) {
            case 'no-speech':
              setError('Keine Sprache erkannt. Bitte sprechen Sie lauter oder näher zum Mikrofon.');
              break;
            case 'audio-capture':
              setError('Mikrofon nicht gefunden. Bitte überprüfen Sie Ihre Einstellungen.');
              break;
            case 'not-allowed':
              setError('Mikrofon-Zugriff wurde verweigert. Bitte erlauben Sie den Zugriff in Ihren Browsereinstellungen.');
              break;
            case 'aborted':
              // Don't show error for aborted - it's usually intentional
              break;
            case 'network':
              setError('Netzwerkfehler bei der Spracherkennung. Die Web Speech API benötigt normalerweise eine Internetverbindung. Bitte überprüfen Sie Ihre Internetverbindung oder verwenden Sie die Texteingabe.');
              break;
            case 'service-not-allowed':
              setError('Spracherkennungsdienst nicht verfügbar. Bitte versuchen Sie es später erneut.');
              break;
            case 'bad-grammar':
              setError('Grammatikfehler in der Spracherkennung.');
              break;
            case 'language-not-supported':
              setError(`Sprache "${language}" wird nicht unterstützt.`);
              break;
            default:
              setError(`Fehler bei der Spracherkennung: ${event.error}${event.message ? ` - ${event.message}` : ''}. Bitte versuchen Sie es erneut.`);
          }
        };

        recognition.onend = () => {
          setIsListening(false);
        };

        return recognition;
      } catch (err) {
        console.error('Error creating speech recognition:', err);
        setError(`Fehler beim Initialisieren der Spracherkennung: ${err instanceof Error ? err.message : 'Unbekannter Fehler'}`);
        return null;
      }
    };

    const recognition = createRecognition();
    if (recognition) {
      recognitionRef.current = recognition;
    }

    return () => {
      if (recognitionRef.current) {
        try {
          recognitionRef.current.stop();
        } catch (err) {
          // Ignore errors when stopping
        }
        recognitionRef.current = null;
      }
    };
  }, [language, onResult]);

  const startListening = () => {
    try {
      if (!recognitionRef.current) {
        setError('Spracherkennung wurde nicht initialisiert. Bitte laden Sie die Seite neu.');
        return;
      }

      if (isListening) {
        return; // Already listening
      }

      setTranscript('');
      setError(null);
      recognitionRef.current.start();
    } catch (err) {
      console.error('Error starting speech recognition:', err);
      setError(`Fehler beim Starten der Spracherkennung: ${err instanceof Error ? err.message : 'Unbekannter Fehler'}`);
      setIsListening(false);
    }
  };

  const stopListening = () => {
    if (recognitionRef.current && isListening) {
      recognitionRef.current.stop();
    }
  };

  const clearTranscript = () => {
    setTranscript('');
    setError(null);
  };

  return {
    isListening,
    transcript,
    error,
    startListening,
    stopListening,
    clearTranscript,
    isSupported: typeof window !== 'undefined' && (window.SpeechRecognition || (window as any).webkitSpeechRecognition),
  };
}

// Type definitions for Web Speech API
interface SpeechRecognition extends EventTarget {
  continuous: boolean;
  interimResults: boolean;
  lang: string;
  start(): void;
  stop(): void;
  onstart: ((this: SpeechRecognition, ev: Event) => any) | null;
  onresult: ((this: SpeechRecognition, ev: SpeechRecognitionEvent) => any) | null;
  onerror: ((this: SpeechRecognition, ev: SpeechRecognitionErrorEvent) => any) | null;
  onend: ((this: SpeechRecognition, ev: Event) => any) | null;
}

interface SpeechRecognitionEvent extends Event {
  resultIndex: number;
  results: SpeechRecognitionResultList;
}

interface SpeechRecognitionErrorEvent extends Event {
  error: string;
  message?: string;
}

interface SpeechRecognitionResultList {
  length: number;
  item(index: number): SpeechRecognitionResult;
  [index: number]: SpeechRecognitionResult;
}

interface SpeechRecognitionResult {
  length: number;
  item(index: number): SpeechRecognitionAlternative;
  [index: number]: SpeechRecognitionAlternative;
  isFinal: boolean;
}

interface SpeechRecognitionAlternative {
  transcript: string;
  confidence: number;
}

declare global {
  interface Window {
    SpeechRecognition: {
      new (): SpeechRecognition;
    };
    webkitSpeechRecognition: {
      new (): SpeechRecognition;
    };
  }
}
