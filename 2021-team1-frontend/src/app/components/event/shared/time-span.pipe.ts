import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeSpan'
})
export class TimeSpanPipe implements PipeTransform {

  transform(timeSpanString: string): string {
    if (timeSpanString) {
      return timeSpanString.slice(0, 5);
    }
  }

}
