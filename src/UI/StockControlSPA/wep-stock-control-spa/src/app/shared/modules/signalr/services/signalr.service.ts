import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, lastValueFrom, takeUntil, tap } from 'rxjs';
import { AuthenticationService } from '../../authentication/services/authentication.service';
import { AppConfigurationService } from '../../app-configuration/services/app-configuration.service';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { signalrMethods } from '../signalr-methods';
import { IProductFlowInfoMessage } from '../interfaces/product-flow-info-message.interface';
import { StorageService } from '../../storage/services/storage.service';
import { NotificationSettingsService } from '../../notification-settings/services/notification-settings.service';
import { notificationSettings } from '../../notification-settings/notification-settings';
import { INotificationSettingItem } from '../../notification-settings/interfaces/notification-setting-item.interface';

@Injectable()
export class SignalrService {
    private readonly destroy$ = new Subject<void>();
    private readonly api: string = 'nt/hubs/notification-hub';
    
    private hubConnection: HubConnection;
    private bffUrl?: string;

    messageReceived = new Subject<any>();

    constructor(
        private authService: AuthenticationService,
        private configurationService: AppConfigurationService,
        private storageService: StorageService,
        private toastr: ToastrService,
    ) {
        if (this.configurationService.isReady){
            this.bffUrl = this.configurationService.serverSettings?.bffUrl;
            this.init();
        }
        else {
            this.configurationService.settingsLoaded$
                .pipe(takeUntil(this.destroy$))
                .subscribe(_ => {
                    this.bffUrl = this.configurationService.serverSettings?.bffUrl;
                    this.init();
                });
        }
    }

    public stop() {
        this.hubConnection.stop();
    }

    private init() {
        if (this.authService.IsAuthorized) {
            this.register();
            this.stablishConnection();
            this.registerHandlers();
        }
    }

    private register() {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(`${this.bffUrl}/${this.api}`, {
                accessTokenFactory: () => this.authService.GetToken()
            })
            .withAutomaticReconnect()
            .build();
    }

    private stablishConnection() {
        this.hubConnection.start()
            .then(() => {
                console.log('Hub connection started')
            })
            .catch(() => {
                console.log('Error while establishing connection')
            });
    }

    private registerHandlers() {
        this.receipts();
        this.movings();     
        this.writeOffs();
    }

    private receipts() {
        this.hubConnection.on(`${signalrMethods.receiptCreated}`, (msg) => {
            console.log(`${signalrMethods.receiptCreated} ${msg.number} ${msg.productFlowId}`);
            const result = this.checkReceiptNotificationSettings();
            if (result) {
                this.toastr.success(`Создано новое поступление с номером: ${msg.number}`);
            }
            this.messageReceived.next({
                productFlowId: msg.productFlowId,
                number: msg.number,
            } as IProductFlowInfoMessage);
        });

        this.hubConnection.on(`${signalrMethods.receiptUpdated}`, (msg) => {
            console.log(`${signalrMethods.receiptUpdated} ${msg.number} ${msg.productFlowId}`);
            const result = this.checkReceiptNotificationSettings();
            if (result) {
                this.toastr.success(`Поступление с номером: ${msg.number} изменено`);
            }
            this.messageReceived.next({
                productFlowId: msg.productFlowId,
                number: msg.number,
            } as IProductFlowInfoMessage);
        });

        this.hubConnection.on(`${signalrMethods.receiptDeleted}`, (msg) => {
            console.log(`${signalrMethods.receiptDeleted} ${msg.number} ${msg.productFlowId}`);
            const result = this.checkReceiptNotificationSettings();
            if (result) {
                this.toastr.success(`Поступление с номером: ${msg.number} удалено`);
            }
            this.messageReceived.next({
                productFlowId: msg.productFlowId,
                number: msg.number,
            } as IProductFlowInfoMessage);
        });

        this.hubConnection.on(`${signalrMethods.receiptBulkDeleted}`, (msg) => {
            console.log(`${signalrMethods.receiptBulkDeleted} ${msg.info}`);
            const result = this.checkReceiptNotificationSettings();
            if (result) {
                const numbers = (msg.info as IProductFlowInfoMessage[]).map(msg => msg.number);
                this.toastr.success(`Поступления с номерами: ${numbers} удалены`);
            }
            this.messageReceived.next(msg.info as IProductFlowInfoMessage[]);
        });
    }

    private checkReceiptNotificationSettings(): boolean {
        const settings = this.storageService.retrieve(notificationSettings.receipts.storageKey);
        if (settings){
            return (settings as INotificationSettingItem).enable;
        }
        else {
            return false;
        }
    }

    private movings() {
        this.hubConnection.on(`${signalrMethods.movingCreated}`, (msg) => {
            console.log(`${signalrMethods.movingCreated} ${msg.number} ${msg.productFlowId}`);
            const result = this.checkMovingNotificationSettings();
            if (result) {
                this.toastr.success(`Создано новое перемещение с номером: ${msg.number}`);
            }
            this.messageReceived.next({
                productFlowId: msg.productFlowId,
                number: msg.number,
            } as IProductFlowInfoMessage);
        });

        this.hubConnection.on(`${signalrMethods.movingUpdated}`, (msg) => {
            console.log(`${signalrMethods.movingUpdated} ${msg.number} ${msg.productFlowId}`);
            const result = this.checkMovingNotificationSettings();
            if (result) {
                this.toastr.success(`Перемещение с номером: ${msg.number} изменено`);
            }
            this.messageReceived.next({
                productFlowId: msg.productFlowId,
                number: msg.number,
            } as IProductFlowInfoMessage);
        });

        this.hubConnection.on(`${signalrMethods.movingDeleted}`, (msg) => {
            console.log(`${signalrMethods.movingDeleted} ${msg.number} ${msg.productFlowId}`);
            const result = this.checkMovingNotificationSettings();
            if (result) {
                this.toastr.success(`Перемещение с номером: ${msg.number} удалено`);
            }
            this.messageReceived.next({
                productFlowId: msg.productFlowId,
                number: msg.number,
            } as IProductFlowInfoMessage);
        });

        this.hubConnection.on(`${signalrMethods.movingBulkDeleted}`, (msg) => {
            console.log(`${signalrMethods.movingBulkDeleted} ${msg.info}`);
            const result = this.checkMovingNotificationSettings();
            if (result) {
                const numbers = (msg.info as IProductFlowInfoMessage[]).map(msg => msg.number);
                this.toastr.success(`Перемещения с номерами: ${numbers} удалены`);
            }
            this.messageReceived.next(msg.info as IProductFlowInfoMessage[]);
        });
    }
    
    private checkMovingNotificationSettings(): boolean {
        const settings = this.storageService.retrieve(notificationSettings.movings.storageKey);
        if (settings){
            return (settings as INotificationSettingItem).enable;
        }
        else {
            return false;
        }
    }

    private writeOffs() {
        this.hubConnection.on(`${signalrMethods.writeOffCreated}`, (msg) => {
            console.log(`${signalrMethods.writeOffCreated} ${msg.number} ${msg.productFlowId}`);
            const result = this.checkWriteOffNotificationSettings();
            if (result) {
                this.toastr.success(`Создано новое списание с номером: ${msg.number}`);
            }
            this.messageReceived.next({
                productFlowId: msg.productFlowId,
                number: msg.number,
            } as IProductFlowInfoMessage);
        });

        this.hubConnection.on(`${signalrMethods.writeOffUpdated}`, (msg) => {
            console.log(`${signalrMethods.writeOffUpdated} ${msg.number} ${msg.productFlowId}`);
            const result = this.checkWriteOffNotificationSettings();
            if (result) {
                this.toastr.success(`Списание с номером: ${msg.number} изменено`);
            }
            this.messageReceived.next({
                productFlowId: msg.productFlowId,
                number: msg.number,
            } as IProductFlowInfoMessage);
        });

        this.hubConnection.on(`${signalrMethods.writeOffDeleted}`, (msg) => {
            console.log(`${signalrMethods.writeOffDeleted} ${msg.number} ${msg.productFlowId}`);
            const result = this.checkWriteOffNotificationSettings();
            if (result) {
                this.toastr.success(`Списание с номером: ${msg.number} удалено`);
            }
            this.messageReceived.next({
                productFlowId: msg.productFlowId,
                number: msg.number,
            } as IProductFlowInfoMessage)
        });

        this.hubConnection.on(`${signalrMethods.writeOffBulkDeleted}`, (msg) => {
            console.log(`${signalrMethods.writeOffBulkDeleted} ${msg.info}`);
            const result = this.checkWriteOffNotificationSettings();
            if (result) {
                const numbers = (msg.info as IProductFlowInfoMessage[]).map(msg => msg.number);
                this.toastr.success(`Списания с номерами: ${numbers} удалены`);
            }
            this.messageReceived.next(msg.info as IProductFlowInfoMessage[]);
        });
    }

    private checkWriteOffNotificationSettings(): boolean {
        const settings = this.storageService.retrieve(notificationSettings.writeOffs.storageKey);
        if (settings){
            return (settings as INotificationSettingItem).enable;
        }
        else {
            return false;
        }
    }
}