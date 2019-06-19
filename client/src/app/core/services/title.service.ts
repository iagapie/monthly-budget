import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { environment } from '../../../environments/environment';
import { TranslateService } from '@ngx-translate/core';

const SEPARATOR = ' - ';

@Injectable({
  providedIn: 'root'
})
export class TitleService {
  private data: string[] = ['APP NAME'];

  constructor(private title: Title, private translate: TranslateService) {
  }

  set(item?: string) {
    this.data = ['APP NAME'];
    this.add(item);
  }

  add(item?: string) {
    if (item) {
      this.data.unshift(item);
    }

    this.apply();
  }

  private apply() {
    this.translate.get(this.data).subscribe(data => {
      const tr: string[] = [];

      for (const key of this.data) {
        tr.push(data[key]);
      }

      this.title.setTitle(tr.join(SEPARATOR));
    });
  }
}
