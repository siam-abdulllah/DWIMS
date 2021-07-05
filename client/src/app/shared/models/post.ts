
export interface IPost {
    id: number;
    postTitle: string;
    postDescription: string;
    postedBy: number;   
    setOn: Date;
    comments: IPostComments[];
}
 
export interface IPostComments {
    commentText: number;
    noOfLike: string;
    noOfDisLike: string;
    commentBy: number;
    postId: string;
    setOn: Date;
}