using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceTravel
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool createImage();

        [OperationContract]
        Traller Login(string email, string password);

        [OperationContract]
        Traller GetTrallerById(int ? id);

        [OperationContract]
        Traller GetTrallerByEmail(string email);

        [OperationContract]
        ImagePost GetImageByUserId(int ? useId);

        [OperationContract]
        Posted PostDetail(int id);

        [OperationContract]
        List<Posted> GetPostsbyUserId(int id);

        [OperationContract]      
        List<Traller> GetTravelers();

        [OperationContract]
        List<ImagePost> SearchImage(int id);

        [OperationContract]
        List<Posted> Search(string name);

        [OperationContract]
        List<PostComment> GetComments();

        [OperationContract]
        List<SubComment> GetSubComments();

        [OperationContract]
        List<PostComment> GetCommentsbyPostId(int id);

        [OperationContract]
        List<SubComment> GetSubCommentsbyCommentId(int id);

        [OperationContract]
        bool PostComment(PostComment postComment);

        [OperationContract]
        bool PostSubComment(SubComment subComment);

        [OperationContract]
        Posted CurrentPost(string name);

        [OperationContract]
        List<ImagePost> GetImages();

        [OperationContract]
        bool ImagePost(ImagePost imagePost, int PostId);

        [OperationContract]
        bool ImageUserPost(ImagePost imagePost, int UsertId);

        [OperationContract]
        List<Posted> GetPosts();

        [OperationContract]
        bool Registor(Traveler traveler);

        [OperationContract]
        bool Post1(string title, string content, int UserId);

        [OperationContract]
        bool Post(Post post, Image image);

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
