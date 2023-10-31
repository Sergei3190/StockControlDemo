import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { StorageService } from '../../storage/services/storage.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConfigurationService } from '../../app-configuration/services/app-configuration.service';

@Injectable()
export class AuthenticationService {
  private readonly destroy$ = new Subject<void>();

  private authorityUrl?: string;
  private headers: HttpHeaders;
  private storage: StorageService;

  private authenticationSource = new Subject<boolean>(); 
  authenticationChallenge$ = this.authenticationSource.asObservable();

  public UserData: any;
  public IsAuthorized?: boolean;

    constructor(private _http: HttpClient,
        private _router: Router,
        private activatedRoute: ActivatedRoute,
        private _configurationService: AppConfigurationService, 
        private _storageService: StorageService) {
        this.headers = new HttpHeaders();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
        this.storage = _storageService;

        if (this._configurationService.isReady){
            this.setAuthorityUrl();
        }
        else {
            this._configurationService.settingsLoaded$
            .pipe(takeUntil(this.destroy$))
            .subscribe(_ => {
                this.setAuthorityUrl();
            });
        }

        if (this.storage.retrieve('IsAuthorized') !== '') {
            this.IsAuthorized = this.storage.retrieve('IsAuthorized');
            this.authenticationSource.next(true);
            this.UserData = this.storage.retrieve('userData');
        }
    }

    public GetToken(): any {
        return this.storage.retrieve('authorizationData');
    }

    public ResetAuthorizationData() {
        this.storage.store('authorizationData', '');
        this.storage.store('authorizationDataIdToken', '');

        this.IsAuthorized = false;
        this.storage.store('IsAuthorized', false);
        console.log(this.storage, this.IsAuthorized);
    }

    public SetAuthorizationData(token: any, id_token: any) {
        if (this.storage.retrieve('authorizationData') !== '') {
            this.storage.store('authorizationData', '');
        }

        this.storage.store('authorizationData', token);
        this.storage.store('authorizationDataIdToken', id_token);
        this.IsAuthorized = true;
        this.storage.store('IsAuthorized', true);

        this._router.navigate(['./']);

        this.getUserData()
            .pipe(
                takeUntil(this.destroy$))
            .subscribe({
                next: data => {
                    this.UserData = data;
                    this.storage.store('userData', data);
                    this.authenticationSource.next(true);
                },
                error: error => this.HandleError(error),
                complete: () => {
                    console.log(this.UserData);
                }
            });
    }

    public Authorize() {
        this.ResetAuthorizationData();

        this.authorityUrl = this.storage.retrieve('IdentityUrl');
        if (!this.authorityUrl){
            return;
        }

        const authorizationUrl = this.authorityUrl + '/connect/authorize';
        const client_id = 'js';
        const redirect_uri = location.origin + '/';
        const response_type = 'id_token token';
        const scope = 'openid profile web.bff.stockcontrol '
            + 'note note.grpc notification personal.cabinet file.storage stock.control';
        const nonce = 'N' + Math.random() + '' + Date.now();

        const state = Date.now() + '' + Math.random();

        this.storage.store('authStateControl', state);
        this.storage.store('authNonce', nonce);

        const url =
            authorizationUrl + '?' +
            'response_type=' + encodeURI(response_type) + '&' +
            'client_id=' + encodeURI(client_id) + '&' +
            'redirect_uri=' + encodeURI(redirect_uri) + '&' +
            'scope=' + encodeURI(scope) + '&' +
            'nonce=' + encodeURI(nonce) + '&' +
            'state=' + encodeURI(state);

        window.location.href = url;
    }

    public AuthorizedCallback() {
        const hash = window.location.hash.substring(1, window.location.hash.length);

        if (hash.includes('/app/wsc/')){
            return;
        }

        const result: any = hash.split('&').reduce(function (result: any, item: string) {
            const parts = item.split('=');
            result[parts[0]] = parts[1];
            return result;
        }, {});

        const currentToken = this.storage.retrieve('authorizationData');
        const currentIdToken = this.storage.retrieve('authorizationDataIdToken');

        if (currentToken === result.access_token && currentIdToken === result.id_token){
            return;
        }

        this.ResetAuthorizationData();

        let token = '';
        let id_token = '';
        let authResponseIsValid = false;

        if (!result.error) {

            if (result.state !== this.storage.retrieve('authStateControl')) {
                console.log('AuthorizedCallback incorrect state');
            } else {
                // в противном случаи извлекаем полученный от Identity.API токен
                token = result.access_token;
                id_token = result.id_token;

                const dataIdToken: any = this.getDataFromToken(id_token);

                if (dataIdToken.nonce !== this.storage.retrieve('authNonce')) {
                    console.log('AuthorizedCallback incorrect nonce');
                } else {
                    this.storage.store('authNonce', '');
                    this.storage.store('authStateControl', '');
                    authResponseIsValid = true;
                    console.log('AuthorizedCallback state and nonce validated, returning access token');
                }
            }
        }

        if (authResponseIsValid) {
            this.SetAuthorizationData(token, id_token);
        }
    }

    public Logoff() {
        const authorizationUrl = this.authorityUrl + '/connect/endsession';
        const id_token_hint = this.storage.retrieve('authorizationDataIdToken');

        const post_logout_redirect_uri = location.origin + '/';

        const url =
            authorizationUrl + '?' +
            'id_token_hint=' + encodeURI(id_token_hint) + '&' +
            'post_logout_redirect_uri=' + encodeURI(post_logout_redirect_uri);

        this.ResetAuthorizationData();

        this.authenticationSource.next(false);

        window.location.href = url;
    }

    public HandleError(error: any) {
        console.log(error);
        // TODO
        if (error.status == 403) {
            this._router.navigate(['/Forbidden']);
        }
        else if (error.status == 401) {
            this._router.navigate(['/Unauthorized']);
        }
    }

    private urlBase64Decode(str: string) {
        let output = str.replace('-', '+').replace('_', '/');

        switch (output.length % 4) {
            case 0:
                break;
            case 2:
                output += '==';
                break;
            case 3:
                output += '=';
                break;
            default:
                throw 'Illegal base64url string!';
        }

        return window.atob(output);
    }

    private getDataFromToken(token: any) {
        let data = {};

        if (typeof token !== 'undefined') {    
            const encoded = token.split('.')[1];     
            data = JSON.parse(this.urlBase64Decode(encoded));
        }

        return data;
    }

    private getUserData = (): Observable<string[]> => {
        if (!this.authorityUrl) {
            this.authorityUrl = this.storage.retrieve('IdentityUrl');
        }

        const options = this.setHeaders();

        return this._http.get<string[]>(`${this.authorityUrl}/connect/userinfo`, options)
            .pipe<string[]>((info: any) => info);
    }

    private setHeaders(): any {
        const httpOptions = {
            headers: new HttpHeaders()
        };

        httpOptions.headers = httpOptions.headers.set('Content-Type', 'application/json');
        httpOptions.headers = httpOptions.headers.set('Accept', 'application/json');

        const token = this.GetToken();

        if (token !== '') {
            httpOptions.headers = httpOptions.headers.set('Authorization', `Bearer ${token}`);
        }

        return httpOptions;
    }

    private setAuthorityUrl() {
        this.authorityUrl = this._configurationService.serverSettings?.identityUrl;
        this.storage.store('IdentityUrl', this.authorityUrl);
    }
}
