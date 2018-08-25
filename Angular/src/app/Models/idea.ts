import { Comment } from './comment';
import { Status } from './status';
import { Theme } from './theme';

export class Idea 
{
    postId: number;
    score: number;
    message: string;
    submitDt: Date;
    modifiedDt: Date;
    owner: string;
    comments: Comment[];
    ideaStatus: Status[];
    themeId: number;
    theme: Theme;
    userVoteDirection: number;
}