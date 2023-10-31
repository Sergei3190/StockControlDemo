import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { INotificationSettingItem } from "./notification-setting-item.interface";

export interface INotificationSettings extends IPaginatedItems {
    items: INotificationSettingItem[];
}