import { Guid } from "guid-ts";
import { IStockAvailabilityItem } from "../interfaces/stock-availability-item.interface";
import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";
import { IParty } from "src/app/shared/interfaces/party.interface";

export class StockAvailabilityItem implements IStockAvailabilityItem {
    id: Guid;
    receiptId?: Guid;
    movingId?: Guid;
    writeOffId?: Guid;
    party: IParty;
    nomenclature: INamedEntity;
    warehouse: INamedEntity;
    organization: INamedEntity;
    price: string;
    quantity = 0;
    totalPrice?: string;
}