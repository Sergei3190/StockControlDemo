import { Inject, Injectable } from '@angular/core';
import { IConfiguration } from '../interfaces/configuration.interface';
import { Subject, takeUntil } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { StorageService } from '../../storage/services/storage.service';

@Injectable()
export class AppConfigurationService {
  private readonly destroy$ = new Subject<void>();

  isReady = false;
  apiUrl?: string;
  serverSettings?: IConfiguration;

  private settingsLoadedSource = new Subject<void>();
  settingsLoaded$ = this.settingsLoadedSource.asObservable();
 
  constructor(private http: HttpClient, private storageService: StorageService, @Inject('API_BASE_URL') baseUrl: string) {
      this.apiUrl = baseUrl;
   }
  
  load() {
      console.log('config');
      const url = `${this.apiUrl}api/config/get`;
      this.http.get(url, {observe: 'response'})
        .pipe(takeUntil(this.destroy$))
        .subscribe({
            next: response => {
                console.log('config set');
                this.serverSettings = response.body as IConfiguration;
                this.storageService.store("bffUrl", this.serverSettings.bffUrl);
                this.isReady = true;
                this.settingsLoadedSource.next();
            },
            error: error => console.error(error)          
        });
    }
}
