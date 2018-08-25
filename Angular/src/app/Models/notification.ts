import { Status } from './status';

export class Notification
{
    id: number;
    createDt: Date;
    statusId: number;
    status: Status;
    isExec: boolean;
}