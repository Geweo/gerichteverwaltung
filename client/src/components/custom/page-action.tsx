import type { ReactNode } from 'react';

import { cn } from '@/lib/utils';

interface Props {
  children?: ReactNode;
  className?: string;
}

export function PageAction({ children, className }: Props) {
  return (
    <div
      data-slot="page-action"
      className={cn('col-start-2 row-span-2 row-start-1 self-start justify-self-end', className)}
    >
      {children}
    </div>
  );
}
