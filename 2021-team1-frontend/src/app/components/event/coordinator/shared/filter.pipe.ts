import { Pipe, PipeTransform } from '@angular/core';
import { cloneDeep } from 'lodash';

import { AppointmentStatus } from '../../../../models';

@Pipe({
  name: 'filter',
})
export class FilterPipe implements PipeTransform {

  transform<T>(items: ReadonlyArray<T>, filterBy: (item: T, values: number[] | AppointmentStatus[]) => T,
               values: number[] | AppointmentStatus[]): ReadonlyArray<T> {
    if (filterBy && values && values.length) {
      return cloneDeep(items)
        .map((item: T) => filterBy(item, values))
        .filter((item: T) => item);
    }

    return cloneDeep(items);
  }

}
