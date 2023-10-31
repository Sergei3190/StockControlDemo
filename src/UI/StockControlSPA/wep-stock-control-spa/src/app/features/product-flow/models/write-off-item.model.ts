import { Guid } from "guid-ts";
import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";
import { IParty } from "src/app/shared/interfaces/party.interface";
import { IWriteOffItem } from "../interfaces/write-off/write-off-item.interface";

export class WriteOffItem implements IWriteOffItem {
    id: Guid;
    productFlowType?: INamedEntity;
    number: string;
    createDate: any;
    createTime: any;
    sendingWarehouse?: INamedEntity;
    party: IParty;
    nomenclature: INamedEntity;
    warehouse: INamedEntity;
    organization: INamedEntity;
    reason?: string;
    price: string;
    quantity = 0;
    totalPrice?: string;
}