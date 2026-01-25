import { cn } from '@/lib/utils';

interface Props {
  className?: string;
  showBar?: boolean;
  message?: string;
}

export function LoadingScreen({ className, showBar = true, message }: Props) {
  return (
    <div
      className={cn(
        'fade-in fixed inset-0 flex animate-in flex-col bg-background transition-opacity duration-400',
        className
      )}
    >
      {/* Main Content - Centered */}
      <div className="flex flex-1 items-center justify-center">
        <div className="flex flex-col items-center justify-center space-y-8">
          {/* Logo/Icon */}
          <div className={cn('h-12 w-12 text-muted-foreground', showBar ? 'animate-none' : 'animate-pulse')}>
            🍳
          </div>

          {showBar && (
            <div className="relative h-1 w-64 overflow-hidden rounded-full bg-muted">
              {/* Animated Progress Bar */}
              <div
                className="absolute inset-0 animate-loading-bar rounded-full bg-muted-foreground"
                style={{
                  animation: 'loading-bar 1.5s cubic-bezier(0.4, 0, 0.2, 1) infinite',
                }}
              />
            </div>
          )}

          {/* Loading Text */}
          {message ? (
            <p className="animate-in fade-in text-sm text-muted-foreground">{message}</p>
          ) : (
            <p className="sr-only">Loading...</p>
          )}
        </div>
      </div>

      {/* Footer */}
      <div className="flex items-center justify-center gap-2 pb-8 text-muted-foreground/50">
        <span className="text-sm">Ernährbär</span>
      </div>

      <style>{`
        @keyframes loading-bar {
          0% {
            transform: translateX(-100%);
          }
          100% {
            transform: translateX(100%);
          }
        }
      `}</style>
    </div>
  );
}
