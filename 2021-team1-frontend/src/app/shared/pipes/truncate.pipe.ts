import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate',
})
export class TruncatePipe implements PipeTransform {

  transform(value: string, limit = 100, ellipsis = '...'): string {
    return value.length > limit
      ? value.substr(0, limit) + ellipsis
      : value;
  }
}
