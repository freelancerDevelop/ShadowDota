﻿using UnityEngine;
using System.Collections;
using AW.Message;
using AW.Data;
using System;

namespace AW.War {

    public class LifeSummonNPC : ServerLifeNpc {


        #region 变量，数值，开关
        /// <summary>
        /// 存活时长
        /// </summary>
        FloatFog lifeTime;

        /// <summary>
        /// 是否初始化
        /// </summary>
        bool inited;
        #endregion

        #region 目标控制
        WarServerManager wmMgr;
        ServerNPC parent;
        #endregion

        #region 回调
        public Action<BNPC> onStart = null;
        public Action<BNPC> onEnd = null;
        public Action<BNPC> onUpdate = null;
        #endregion

    	// Use this for initialization
        public override void Start () {
            base.Start();
    	}
    	
    	// Update is called once per frame
        public void Update () {
            if(inited)
            {
                lifeTime -= Time.deltaTime;
                if(lifeTime < 0f)
                {
                    inited = false;
                    WarTarAnimParam deadParam = new WarTarAnimParam ();
                    deadParam.OP = EffectOp.Injury;

                    SelfDescribed des = new SelfDescribed() {
                        src = UniqueID,
                        target = UniqueID,
                        act = Verb.Punch,
                        srcEnd = null,
                        targetEnd = new EndResult() {
                            param1 = 9999999,
                            param2 = 0,
                            param3 = 0,
                            param4 = 0,
                        },
                    };

                    deadParam.described = des;

                    deadParam.Sender = UniqueID;
                    deadParam.Receiver = UniqueID;

                    wmMgr.npcMgr.SendMessage(UniqueID, UniqueID, deadParam);
                }
            }
    	}

        public override void Init(ServerNPC owner, WarMsgParam param)
        {
            parent = owner;
            ConsoleEx.DebugLog(parent.name);
            wmMgr = WarServerManager.Instance;
            UniqueId = wmMgr.npcMgr.SignID(this);
            Camp = Camp.set(owner.Camp);
			WarSrcAnimParam wp = param as WarSrcAnimParam;
            if(wp != null)
            {
                SelfDescribed sd = wp.described;
                EndResult result = sd.srcEnd;
                lifeTime = result.param8;
            }
            inited = true;
        }

        #region override
        public override void OnHandleMessage(MsgParam param)
        {
            base.OnHandleMessage(param);
        }

        public override KindOfNPC WhatKindOf()
        {
            return KindOfNPC.Life;
        }
        #endregion
    }

}