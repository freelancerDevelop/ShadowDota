using System;
using xClient;
using System.Collections.Generic;

namespace xClient.Action{
	/**
	 * ��ʼ������
	 */
	public partial class AllActClass{
		
		public Dictionary<string, IAction> dic = new Dictionary<string, IAction>();
		
		public AllActClass()
		{
			dic.Add("UserAction", new xClient.Action.UserAction());
		}
	}
}