import { Guid } from "guid-ts";
import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";
import { IParty } from "src/app/shared/interfaces/party.interface";
import { IMovingItem } from "../interfaces/moving/moving-item.interface";

export class MovingItem implements IMovingItem {
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
    price: string;
    quantity = 0;
    totalPrice?: string;
}