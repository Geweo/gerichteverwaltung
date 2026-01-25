import type { ReactNode } from 'react';

import { cn } from '@/lib/utils';

interface Props {
  children?: ReactNode;
  className?: string;
}

export function PageHeader({ children, className }: Props) {
  return (
    <div
      className={cn(
        'border-b px-4 pt-1 pb-2 md:pt-2 md:pb-4 lg:px-6',
        'grid auto-rows-min grid-rows-[auto_auto] items-start px-6 has-data-[slot=page-action]:grid-cols-[1fr_auto] has-data-[slot=page-description]:gap-2',
        className
      )}
    >
      {children}
    </div>
  );
}
