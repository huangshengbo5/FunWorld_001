

CREATE TABLE `game_hero_equip` (
  `id` int(11) UNSIGNED NOT NULL COMMENT 'ID',
  `uid` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `equip_id` int(11) UNSIGNED NOT NULL COMMENT '装备ID',
  `name` varchar(16) NOT NULL COMMENT '装备名称',
  `level` smallint(5) UNSIGNED NOT NULL COMMENT '装备等级',
  `forge_id` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '锻造ID',
  `pos` tinyint(2) UNSIGNED NOT NULL DEFAULT '0' COMMENT '装备部位',
  `fighting_capacity_base` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '战力',
  `fighting_capacity_plus` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '升级附加战力',
  `skill_id` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '装备技能ID',
  `skill_effect1` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '技能效果值1',
  `skill_effect2` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '技能效果值2',
  `skill_effect3` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '技能效果值3',
  `effect_id1` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '二级属性ID',
  `effect_val1` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '属性值',
  `effect_id2` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '二级属性ID',
  `effect_val2` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '属性值',
  `effect_id3` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '二级属性ID',
  `effect_val3` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '属性值',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='伙伴装备表';




CREATE TABLE `game_hero_equip_doc` (
  `id` int(11) UNSIGNED NOT NULL COMMENT '主键',
  `uid` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `equip_id` int(11) UNSIGNED NOT NULL COMMENT '装备ID',
  `has_receive` tinyint(2) UNSIGNED NOT NULL DEFAULT '0' COMMENT '是否领奖0否 1是',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='装备图鉴';



CREATE TABLE `game_passport` (
  `id` int(10) UNSIGNED NOT NULL COMMENT 'passport_id',
  `name` varchar(64) NOT NULL COMMENT 'passport_name',
  `login_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '登陆时间',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='账号表';


CREATE TABLE `game_user` (
  `id` int(10) UNSIGNED NOT NULL COMMENT '用户ID',
  `name` varchar(32) NOT NULL COMMENT '用户名',
  `level` smallint(5) UNSIGNED NOT NULL DEFAULT '1' COMMENT '等级',
  `avatar` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '头像',
  `ftue` tinyint(2) UNSIGNED NOT NULL DEFAULT '0' COMMENT '新手引导步骤号',
  `main_hero_id` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '主将ID',
  `sub_hero_id` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '出战伙伴ID',
  `gold` bigint(20) UNSIGNED NOT NULL DEFAULT '0' COMMENT '金币',
  `gold_flush_in` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '金币最新结算时间',
  `gold_buff_end_time` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '双倍金币结束时间',
  `diamond` int(10) UNSIGNED NOT NULL DEFAULT '0' COMMENT '钻石',
  `resource` blob NOT NULL COMMENT '其他资源',
  `attr` blob COMMENT '二级属性',
  `equips` blob NOT NULL COMMENT '装备信息',
  `campaign_num` int(10) UNSIGNED NOT NULL DEFAULT '0' COMMENT '通过的关卡数',
  `building_effect` blob NOT NULL COMMENT '建筑效果',
  `tech_effect` blob NOT NULL COMMENT '科技效果',
  `cast_effect` tinyblob NOT NULL COMMENT '锻造信息',
  `data_version` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '数据版本号',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户表';



CREATE TABLE `game_user_annals` (
  `id` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `done_list` blob NOT NULL COMMENT '已领取奖励',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='成就表';



CREATE TABLE `game_user_arena` (
  `id` int(11) UNSIGNED NOT NULL COMMENT '唯一ID',
  `uid` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `is_player` tinyint(1) UNSIGNED NOT NULL DEFAULT '0' COMMENT '是否是用户',
  `name` varchar(32) NOT NULL DEFAULT '' COMMENT '名称',
  `avatar` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '头像',
  `level` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '等级',
  `fighting_capacity` bigint(20) UNSIGNED NOT NULL DEFAULT '0' COMMENT '战斗力',
  `sign_week` mediumint(6) UNSIGNED NOT NULL DEFAULT '0' COMMENT '参加周',
  `group_id` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '分组id',
  `rank` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '排名',
  `send_reward` tinyint(1) UNSIGNED NOT NULL DEFAULT '0' COMMENT '已经发奖',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='竞技场表';



CREATE TABLE `game_user_building` (
  `ID` int(11) UNSIGNED NOT NULL COMMENT '主键',
  `uid` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `building_id` int(11) UNSIGNED NOT NULL COMMENT '建筑ID',
  `level` smallint(5) NOT NULL COMMENT '当前等级',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户建筑表';



CREATE TABLE `game_user_crystal` (
  `ID` int(11) UNSIGNED NOT NULL COMMENT '主键',
  `uid` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `crystal_id` int(11) UNSIGNED NOT NULL COMMENT '水晶ID',
  `level` smallint(5) NOT NULL COMMENT '当前等级',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户水晶表';



CREATE TABLE `game_user_explore` (
  `id` int(10) UNSIGNED NOT NULL COMMENT '主键',
  `uid` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `explore_id` tinyint(2) UNSIGNED NOT NULL COMMENT '探索ID',
  `start_time` int(11) UNSIGNED NOT NULL COMMENT '开始时间',
  `hero_ids` blob NOT NULL COMMENT '探索详情',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户探索表';




CREATE TABLE `game_user_extend` (
  `id` int(11) UNSIGNED NOT NULL COMMENT '用户id',
  `campaign_id` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '关卡ID',
  `campaign_time` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '通关时间',
  `campaign_old_id` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '旧的关卡ID',
  `online_reward` tinyblob NOT NULL COMMENT '在线奖励数据',
  `apocalypse` tinyblob NOT NULL COMMENT '天启数据',
  `tower` blob NOT NULL COMMENT '怪物入侵数据',
  `arena_remain_num` tinyint(1) UNSIGNED NOT NULL DEFAULT '0' COMMENT '竞技场剩余次数',
  `arena_sign_week` mediumint(6) UNSIGNED NOT NULL DEFAULT '0' COMMENT '竞技场报名周',
  `alchemy` tinyblob COMMENT '炼药',
  `business_man` tinyint(2) UNSIGNED NOT NULL DEFAULT '0' COMMENT '商人记录',
  `ads` tinyblob NOT NULL COMMENT '广告数据',
  `last_modify_day` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '最后重置日期Ymd',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户扩展信息表';




CREATE TABLE `game_user_hero` (
  `id` int(11) UNSIGNED NOT NULL COMMENT '主键ID',
  `uid` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `hero_id` int(11) UNSIGNED NOT NULL COMMENT '英雄ID',
  `type` tinyint(2) UNSIGNED NOT NULL COMMENT '英雄类型1主角 2伙伴',
  `name` varchar(16) NOT NULL COMMENT '英雄名',
  `level` smallint(5) UNSIGNED NOT NULL DEFAULT '1' COMMENT '等级',
  `evolve_times` tinyint(2) UNSIGNED NOT NULL DEFAULT '0' COMMENT '突破次数',
  `fighting_capacity` int(11) UNSIGNED NOT NULL DEFAULT '0' COMMENT '战斗力',
  `attack_freq` smallint(5) UNSIGNED NOT NULL COMMENT '基础攻击频率',
  `attack_trans` smallint(5) UNSIGNED NOT NULL COMMENT '攻击转换比例',
  `defend_trans` smallint(5) UNSIGNED NOT NULL COMMENT '防御转换比例',
  `hp_trans` smallint(5) UNSIGNED NOT NULL COMMENT '血量转化率',
  `attack_ratio` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '锻魂攻击加成',
  `defend_ratio` smallint(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT '锻魂防御加成',
  `sex` tinyint(2) UNSIGNED NOT NULL DEFAULT '0' COMMENT '性别',
  `race` tinyint(2) UNSIGNED NOT NULL DEFAULT '0' COMMENT '种族',
  `skills` tinyblob NOT NULL COMMENT '技能信息',
  `skin_id` int(11) UNSIGNED NOT NULL COMMENT '皮肤ID',
  `skin_map` tinyblob NOT NULL COMMENT '皮肤数组',
  `explore_id` tinyint(2) UNSIGNED NOT NULL DEFAULT '0' COMMENT '探索ID',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='英雄表';



CREATE TABLE `game_user_mail` (
  `id` int(11) UNSIGNED NOT NULL COMMENT '主键',
  `uid` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `mail_id` int(11) UNSIGNED NOT NULL COMMENT '邮件',
  `params` tinyblob COMMENT '参数',
  `status` tinyint(1) UNSIGNED NOT NULL DEFAULT '0' COMMENT '状态0未读1已读2删除',
  `has_received` tinyint(1) UNSIGNED DEFAULT '0' COMMENT '附件状态0未领取1已领取',
  `attachment` varchar(128) NOT NULL DEFAULT '' COMMENT '附件内容',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户邮件';



CREATE TABLE `game_user_tech` (
  `ID` int(11) UNSIGNED NOT NULL COMMENT '主键',
  `uid` int(11) UNSIGNED NOT NULL COMMENT '用户ID',
  `tech_id` int(11) UNSIGNED NOT NULL COMMENT '科技ID',
  `level` smallint(6) NOT NULL COMMENT '当前等级',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户科技表';




--
-- 表的索引 `game_hero_equip`
--
ALTER TABLE `game_hero_equip`
  ADD PRIMARY KEY (`id`),
  ADD KEY `uid` (`uid`);

--
-- 表的索引 `game_hero_equip_doc`
--
ALTER TABLE `game_hero_equip_doc`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uid` (`uid`,`equip_id`);

--
-- 表的索引 `game_passport`
--
ALTER TABLE `game_passport`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `name` (`name`);

--
-- 表的索引 `game_user`
--
ALTER TABLE `game_user`
  ADD PRIMARY KEY (`id`);

--
-- 表的索引 `game_user_annals`
--
ALTER TABLE `game_user_annals`
  ADD PRIMARY KEY (`id`);

--
-- 表的索引 `game_user_arena`
--
ALTER TABLE `game_user_arena`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uid` (`uid`,`is_player`),
  ADD KEY `group_id` (`group_id`,`sign_week`);

--
-- 表的索引 `game_user_building`
--
ALTER TABLE `game_user_building`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uid` (`uid`,`building_id`);

--
-- 表的索引 `game_user_crystal`
--
ALTER TABLE `game_user_crystal`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uid` (`uid`,`crystal_id`);

--
-- 表的索引 `game_user_explore`
--
ALTER TABLE `game_user_explore`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uid` (`uid`,`explore_id`);

--
-- 表的索引 `game_user_extend`
--
ALTER TABLE `game_user_extend`
  ADD PRIMARY KEY (`id`);

--
-- 表的索引 `game_user_hero`
--
ALTER TABLE `game_user_hero`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uid` (`uid`,`hero_id`);

--
-- 表的索引 `game_user_mail`
--
ALTER TABLE `game_user_mail`
  ADD PRIMARY KEY (`id`);

--
-- 表的索引 `game_user_tech`
--
ALTER TABLE `game_user_tech`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uid` (`uid`,`tech_id`);

--
-- 在导出的表使用AUTO_INCREMENT
--


--
-- 使用表AUTO_INCREMENT `game_hero_equip`
--
ALTER TABLE `game_hero_equip`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'ID', AUTO_INCREMENT=1;

--
-- 使用表AUTO_INCREMENT `game_hero_equip_doc`
--
ALTER TABLE `game_hero_equip_doc`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键', AUTO_INCREMENT=1;

--
-- 使用表AUTO_INCREMENT `game_passport`
--
ALTER TABLE `game_passport`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'passport_id', AUTO_INCREMENT=1;

--
-- 使用表AUTO_INCREMENT `game_user_arena`
--
ALTER TABLE `game_user_arena`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '唯一ID';

--
-- 使用表AUTO_INCREMENT `game_user_building`
--
ALTER TABLE `game_user_building`
  MODIFY `ID` int(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键', AUTO_INCREMENT=1;

--
-- 使用表AUTO_INCREMENT `game_user_crystal`
--
ALTER TABLE `game_user_crystal`
  MODIFY `ID` int(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键', AUTO_INCREMENT=1;

--
-- 使用表AUTO_INCREMENT `game_user_explore`
--
ALTER TABLE `game_user_explore`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键', AUTO_INCREMENT=1;

--
-- 使用表AUTO_INCREMENT `game_user_hero`
--
ALTER TABLE `game_user_hero`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键ID', AUTO_INCREMENT=1;

--
-- 使用表AUTO_INCREMENT `game_user_mail`
--
ALTER TABLE `game_user_mail`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键', AUTO_INCREMENT=1;

--
-- 使用表AUTO_INCREMENT `game_user_tech`
--
ALTER TABLE `game_user_tech`
  MODIFY `ID` int(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键', AUTO_INCREMENT=1;
