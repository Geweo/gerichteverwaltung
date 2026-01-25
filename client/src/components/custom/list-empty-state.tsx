import type React from 'react';
import { InboxIcon, SearchIcon } from 'lucide-react';

import { Empty, EmptyContent, EmptyDescription, EmptyHeader, EmptyMedia, EmptyTitle } from '@/components/ui/empty';

interface ListEmptyStateProps {
  // State
  isFiltered: boolean;
  searchValue?: string;

  // Customization
  icon?: React.ReactNode;
  emptyTitle?: string;
  emptyDescription?: string;
  filteredIcon?: React.ReactNode;
  filteredTitle?: string;
  filteredDescription?: string;

  // Composable actions
  actions?: React.ReactNode;
  filteredActions?: React.ReactNode;
}

export function ListEmptyState({
  isFiltered,
  searchValue,
  icon,
  emptyTitle = 'No data available',
  emptyDescription = 'No entries have been created yet',
  filteredIcon,
  filteredTitle,
  filteredDescription = 'Try different search terms or adjust the filters',
  actions,
  filteredActions,
}: ListEmptyStateProps) {
  // Filtered State (Search/Filter active)
  if (isFiltered) {
    const title = filteredTitle || (searchValue ? `No results for "${searchValue}"` : 'No results found');

    return (
      <Empty>
        <EmptyHeader>
          <EmptyMedia variant="icon">{filteredIcon || icon || <SearchIcon />}</EmptyMedia>
          <EmptyTitle>{title}</EmptyTitle>
          {filteredDescription && <EmptyDescription>{filteredDescription}</EmptyDescription>}
        </EmptyHeader>
        {filteredActions && <EmptyContent>{filteredActions}</EmptyContent>}
      </Empty>
    );
  }

  // Empty State (No data at all)
  return (
    <Empty>
      <EmptyHeader>
        <EmptyMedia variant="icon">{icon || <InboxIcon />}</EmptyMedia>
        <EmptyTitle>{emptyTitle}</EmptyTitle>
        {emptyDescription && <EmptyDescription>{emptyDescription}</EmptyDescription>}
      </EmptyHeader>
      {actions && <EmptyContent>{actions}</EmptyContent>}
    </Empty>
  );
}
