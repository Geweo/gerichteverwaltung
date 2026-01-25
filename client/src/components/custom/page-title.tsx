import type { ReactNode } from 'react';

import { cn } from '@/lib/utils';
import { Skeleton } from '@/components/ui/skeleton';

type Variant = 'h1' | 'h2' | 'h3';

interface Props {
  children?: ReactNode;
  icon?: ReactNode;
  variant?: Variant;
  className?: string;
  iconClassName?: string;
  isLoading?: boolean;
}

const variants: Record<Variant, string> = {
  h1: 'text-2xl font-semibold tracking-tight',
  h2: 'text-xl font-semibold tracking-tight',
  h3: 'text-lg font-semibold tracking-tight',
};

const iconVariants: Record<Variant, string> = {
  h1: 'size-6',
  h2: 'size-5',
  h3: 'size-4',
};

export function PageTitle({ children, icon, variant = 'h1', className, iconClassName, isLoading }: Props) {
  const Component = variant;

  return (
    <Component className={cn('flex items-center gap-2', variants[variant], className)}>
      {icon && (
        <span aria-hidden="true" className={cn('inline-flex items-center', iconVariants[variant], iconClassName)}>
          {icon}
        </span>
      )}
      {isLoading ? <Skeleton className="h-4 w-48" /> : children}
    </Component>
  );
}
