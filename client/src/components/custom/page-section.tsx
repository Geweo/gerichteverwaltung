import type { ReactNode } from 'react';

import { cn } from '@/lib/utils';

interface Props {
  children?: ReactNode;
  className?: string;
  padding?: boolean;
}

export function PageSection({ children, className, padding = true }: Props) {
  return <section className={cn(padding && 'px-4 lg:px-6', className)}>{children}</section>;
}
