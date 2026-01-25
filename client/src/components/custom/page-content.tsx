import type { ReactNode } from 'react';

import { cn } from '@/lib/utils';

interface Props {
  children?: ReactNode;
  className?: string;
}

export function PageContent({ children, className }: Props) {
  return <div className={cn('flex flex-col gap-6 overflow-y-auto py-4 md:gap-8 md:py-6', className)}>{children}</div>;
}
