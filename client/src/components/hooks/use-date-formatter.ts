import { useMemo } from 'react';
import { format, formatDistance, formatRelative, isValid, type Locale } from 'date-fns';
import { de, enUS } from 'date-fns/locale';

interface FormatDateOptions {
  pattern?: string;
  includeTime?: boolean;
}

type LanguageCode = 'en-US' | 'de-DE';

export const useDateFormatter = () => {
  // TODO: Integrate with i18n when available
  const currentLocale: LanguageCode = 'de-DE';

  const dateLocale = useMemo(() => {
    const localeMap: Record<LanguageCode, Locale> = {
      'en-US': enUS,
      'de-DE': de,
    };
    return localeMap[currentLocale] || de;
  }, [currentLocale]);

  const formatDate = useMemo(() => {
    return (date: Date | string | number, options: FormatDateOptions = {}): string => {
      const { pattern, includeTime = false } = options;
      const dateObj = new Date(date);

      if (!isValid(dateObj)) {
        return String(date);
      }

      const defaultPattern = includeTime ? 'PPp' : 'PP';
      const formatPattern = pattern || defaultPattern;

      return format(dateObj, formatPattern, { locale: dateLocale });
    };
  }, [dateLocale]);

  const formatShortDate = useMemo(() => {
    return (date: Date | string | number): string => formatDate(date, { pattern: 'P' });
  }, [formatDate]);

  const formatLongDate = useMemo(() => {
    return (date: Date | string | number): string => formatDate(date, { pattern: 'PPP' });
  }, [formatDate]);

  const formatDateTime = useMemo(() => {
    return (date: Date | string | number): string => formatDate(date, { includeTime: true });
  }, [formatDate]);

  const formatSystemDateTime = useMemo(() => {
    return (date: Date | string | number): string => formatDate(date, { pattern: 'yyyy-MM-dd HH:mm:ss' });
  }, [formatDate]);

  const formatRelativeTime = useMemo(() => {
    return (date: Date | string | number): string => {
      const dateObj = new Date(date);

      if (!isValid(dateObj)) {
        return String(date);
      }

      return formatRelative(dateObj, new Date(), { locale: dateLocale });
    };
  }, [dateLocale]);

  const formatTimeAgo = useMemo(() => {
    return (date: Date | string | number): string => {
      const dateObj = new Date(date);

      if (!isValid(dateObj)) {
        return String(date);
      }

      return formatDistance(dateObj, new Date(), {
        addSuffix: true,
        locale: dateLocale,
      });
    };
  }, [dateLocale]);

  return {
    formatDate,
    formatShortDate,
    formatLongDate,
    formatDateTime,
    formatSystemDateTime,
    formatRelativeTime,
    formatTimeAgo,
    currentLocale,
    dateLocale,
  };
};
