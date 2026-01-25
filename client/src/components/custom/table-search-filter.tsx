import { useEffect, useState } from 'react';

import { SearchInput } from '@/components/custom/search-input';
import { useDebounce } from '@/components/hooks/use-debounce';
import { cn } from '@/lib/utils';

interface TableSearchFilterProps {
  value?: string;
  onChange: (value: string | undefined) => void;
  placeholder?: string;
  className?: string;
}

export function TableSearchFilter({ value = '', onChange, placeholder, className }: TableSearchFilterProps) {
  const [searchValue, setSearchValue] = useState(value);

  // Sync with prop when it changes externally
  useEffect(() => {
    setSearchValue(value);
  }, [value]);

  // Debounce search updates (500ms delay)
  useDebounce(
    () => {
      onChange(searchValue || undefined);
    },
    500,
    [searchValue]
  );

  return (
    <SearchInput
      value={searchValue}
      onChange={setSearchValue}
      placeholder={placeholder}
      className={cn(className, 'w-full sm:w-96')}
    />
  );
}
