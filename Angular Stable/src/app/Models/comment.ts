export class Comment {
    commentId: number;
    ParentCommentId: number;
    ParentIdeaId: number;
    score: number;
    message: string;
    submitDt: Date;
    modifiedDt: Date;
    owner: string;
    comments: Comment[];
    hasBeenLoaded: boolean = false;
    userVoteDirection: number;
}