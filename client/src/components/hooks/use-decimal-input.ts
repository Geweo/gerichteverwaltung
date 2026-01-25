import { useEffect, useState, type ChangeEvent } from 'react';

export type DecimalField = {
  value: number | undefined;
  onChange: (value: number | undefined) => void;
};

type UseDecimalInputResult = {
  inputValue: string;
  handleChange: (event: ChangeEvent<HTMLInputElement>) => void;
  handleBlur: () => void;
};

export function useDecimalInput(field: DecimalField): UseDecimalInputResult {
  const [inputValue, setInputValue] = useState(field.value?.toString() ?? '');

  useEffect(() => {
    setInputValue(field.value?.toString() ?? '');
  }, [field.value]);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    const filteredValue = value.replace(/[^\d.,-]/g, '');
    setInputValue(filteredValue);
    if (filteredValue === '') {
      field.onChange(undefined);
      return;
    }
    const normalizedValue = filteredValue.replace(',', '.');
    const numValue = Number(normalizedValue);
    if (!Number.isNaN(numValue)) {
      field.onChange(numValue);
    }
  };

  const handleBlur = () => {
    const normalizedValue = inputValue.replace(',', '.');
    const numValue = Number(normalizedValue);
    if (!Number.isNaN(numValue)) {
      setInputValue(numValue.toString());
      field.onChange(numValue);
    } else if (inputValue !== '') {
      setInputValue(field.value?.toString() ?? '');
    }
  };

  return {
    inputValue,
    handleChange,
    handleBlur,
  };
}
