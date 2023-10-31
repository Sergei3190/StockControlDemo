import { Guid } from "guid-ts";
import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";
import { IParty } from "src/app/shared/interfaces/party.interface";
import { IReceiptItem } from "../interfaces/receipt/receipt-item.interface";

export class ReceiptItem implements IReceiptItem {
    id: Guid;
    productFlowType?: INamedEntity;
    number: string;
    createDate: any;
    createTime: any;
    party: IParty;
    nomenclature: INamedEntity;
    warehouse: INamedEntity;
    organization: INamedEntity;
    price: string;
    quantity = 0;
    totalPrice?: string;
}