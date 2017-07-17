using System;
namespace file.Models
{
    /// <summary>
    /// userModel:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>

    public partial class userModel
    {
        public userModel()
        { }
        #region Model
        private int _id;
        private string _name;
        private string _password;
        private string _phone;
        private DateTime _lastlogintime = Convert.ToDateTime("0000 - 00 - 00 00:00:00");
        private DateTime _updatetime = Convert.ToDateTime("0000 - 00 - 00 00:00:00");
        private int _state = 0;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime LastLoginTime
        {
            set { _lastlogintime = value; }
            get { return _lastlogintime; }
        }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        #endregion Model

    }
}

