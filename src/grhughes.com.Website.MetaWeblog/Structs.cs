namespace grhughes.com.Website.MetaWeblog
{
  using System;
  using CookComputing.XmlRpc;

  #region Structs

  public struct BlogInfo
  {
    public string blogName;
    public string blogid;
    public string url;
  }

  public struct Category
  {
    public string categoryId;
    public string categoryName;
  }

  [Serializable]
  public struct CategoryInfo
  {
    public string categoryid;
    public string description;
    public string htmlUrl;
    public string rssUrl;
    public string title;
  }

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct Enclosure
  {
    public int length;
    public string type;
    public string url;
  }

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct Post
  {
    public string[] categories;
    public DateTime dateCreated;
    public string description;
    public string permalink;
    public object postid;
    public string title;
    public string userid;
    public string wp_slug;
  }

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct Source
  {
    public string name;
    public string url;
  }

  public struct UserInfo
  {
    public string email;
    public string firstname;
    public string lastname;
    public string nickname;
    public string url;
    public string userid;
  }

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct MediaObject
  {
    public byte[] bits;
    public string name;
    public string type;
  }

  [Serializable]
  public struct MediaObjectInfo
  {
    public string url;
  }

  #endregion
}