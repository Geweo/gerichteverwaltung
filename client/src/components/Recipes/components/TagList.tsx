import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';

interface TagListProps
{
  tags: string[];
}

export function TagList({ tags }: TagListProps)
{
  if (tags.length === 0)
  {
    return null;
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-lg">Erkannte Tags</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="flex flex-wrap gap-2">
          {tags.map((tag) => (
            <span
              key={tag}
              className="px-3 py-1 bg-primary/10 text-primary rounded-full text-sm font-medium"
            >
              {tag}
            </span>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}
