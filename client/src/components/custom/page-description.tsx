import type { PropsWithChildren, ReactNode } from 'react';

import { cn } from '@/lib/utils';

interface Props {
  children?: ReactNode;
  className?: string;
}

export function PageDescription({ children, className }: PropsWithChildren<Props>) {
  return (
    <div data-slot="page-description" className={cn('text-muted-foreground text-sm leading-normal', className)}>
      {children}
    </div>
  );
}
