import { useState } from 'react';
import * as React from 'react';
import { Star } from 'lucide-react';
import { cn } from '@/lib/utils';
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from '@/components/ui/tooltip';

interface StarRatingProps {
  rating?: number; // User's own rating (1-5)
  averageRating?: number; // Average rating from all users
  ratingCount?: number; // Number of ratings
  onRate?: (rating: number) => void;
  disabled?: boolean;
  size?: 'sm' | 'md' | 'lg';
  showAverage?: boolean; // Show average rating in tooltip
}

/**
 * Interactive star rating component.
 * Shows user's rating (if set) and allows clicking to rate.
 */
export function StarRating({
  rating,
  averageRating,
  ratingCount,
  onRate,
  disabled = false,
  size = 'md',
  showAverage = true,
}: StarRatingProps) {
  const [hoveredRating, setHoveredRating] = useState<number | null>(null);
  const [localRating, setLocalRating] = useState<number | null>(rating ?? null);

  // Update local rating when prop changes (after mutation completes)
  React.useEffect(() => {
    if (rating !== undefined) {
      setLocalRating(rating);
    }
  }, [rating]);

  const sizeClasses = {
    sm: 'h-3 w-3',
    md: 'h-4 w-4',
    lg: 'h-5 w-5',
  };

  const displayRating = hoveredRating ?? localRating ?? 0;
  const hasRating = localRating !== null && localRating > 0;

  const handleClick = (value: number) => {
    if (!disabled && onRate) {
      // Optimistic update: update local state immediately
      const newRating = localRating === value ? 0 : value;
      setLocalRating(newRating > 0 ? newRating : null);
      
      // Toggle: if clicking the same rating, remove it
      if (localRating === value) {
        onRate(0); // 0 means remove rating
      } else {
        onRate(value);
      }
    }
  };

  const stars = Array.from({ length: 5 }, (_, i) => i + 1);

  const content = (
    <div className="flex items-center gap-0.5">
      {stars.map((value) => {
        const isFilled = value <= displayRating;
        const isActive = !disabled && onRate;
        
        return (
          <button
            key={value}
            type="button"
            disabled={disabled || !onRate}
            onClick={() => handleClick(value)}
            onMouseEnter={() => !disabled && setHoveredRating(value)}
            onMouseLeave={() => setHoveredRating(null)}
            className={cn(
              'transition-colors',
              isActive && 'cursor-pointer hover:scale-110',
              disabled && 'cursor-default',
              !isActive && 'cursor-default'
            )}
          >
            <Star
              className={cn(
                sizeClasses[size],
                isFilled
                  ? 'fill-yellow-400 text-yellow-400'
                  : 'fill-transparent text-muted-foreground',
                isActive && !isFilled && 'hover:text-yellow-300'
              )}
            />
          </button>
        );
      })}
      {showAverage && averageRating !== undefined && averageRating !== null && (
        <span className="ml-2 text-xs text-muted-foreground">
          ({averageRating.toFixed(1)})
        </span>
      )}
    </div>
  );

  if (showAverage && (averageRating !== undefined || ratingCount !== undefined)) {
    const tooltipText = [
      averageRating !== undefined && averageRating !== null
        ? `Durchschnitt: ${averageRating.toFixed(1)}`
        : null,
      ratingCount !== undefined && ratingCount !== null
        ? `${ratingCount} Bewertung${ratingCount !== 1 ? 'en' : ''}`
        : null,
      hasRating && localRating !== null && localRating > 0
        ? `Deine Bewertung: ${localRating}`
        : null,
    ]
      .filter(Boolean)
      .join(' • ');

    return (
      <TooltipProvider>
        <Tooltip>
          <TooltipTrigger asChild>
            {content}
          </TooltipTrigger>
          <TooltipContent>
            <p>{tooltipText}</p>
          </TooltipContent>
        </Tooltip>
      </TooltipProvider>
    );
  }

  return content;
}
