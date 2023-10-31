import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {
  private storage: any; // данные будут храниться на клиенте, пока пользователь не закроет вкладку или браузер

  constructor() {
    this.storage = sessionStorage; // localStorage; https://dev-academy.com/angular-session-storage/
  }

  public retrieve(key: string): any {
    const item = this.storage.getItem(key);

    if (item && item !== 'undefined') {
        return JSON.parse(this.storage.getItem(key)); 
    }

    return;
  }

  public store(key: string, value: any) {
    this.storage.setItem(key, JSON.stringify(value));
  }
}
