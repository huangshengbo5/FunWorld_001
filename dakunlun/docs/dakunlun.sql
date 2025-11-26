-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- 主机： 127.0.0.1
-- 生成日期： 2024-10-01 12:06:01
-- 服务器版本： 8.0.39
-- PHP 版本： 7.2.34

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 数据库： `dakunlun`
--

-- --------------------------------------------------------

--
-- 表的结构 `game_hero_equip`
--

DROP TABLE IF EXISTS `game_hero_equip`;
CREATE TABLE `game_hero_equip` (
  `id` int UNSIGNED NOT NULL COMMENT 'ID',
  `uid` int UNSIGNED NOT NULL COMMENT '用户ID',
  `equip_id` int UNSIGNED NOT NULL COMMENT '装备ID',
  `name` varchar(16) NOT NULL COMMENT '装备名称',
  `level` smallint UNSIGNED NOT NULL COMMENT '装备等级',
  `forge_id` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '锻造ID',
  `pos` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '装备部位',
  `fighting_capacity_base` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '战力',
  `fighting_capacity_plus` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '升级附加战力',
  `skill_id` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '装备技能ID',
  `skill_effect1` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '技能效果值1',
  `skill_effect2` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '技能效果值2',
  `skill_effect3` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '技能效果值3',
  `effect_id1` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '二级属性ID',
  `effect_val1` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '属性值',
  `effect_id2` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '二级属性ID',
  `effect_val2` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '属性值',
  `effect_id3` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '二级属性ID',
  `effect_val3` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '属性值',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='伙伴装备表';

-- --------------------------------------------------------

--
-- 表的结构 `game_hero_equip_doc`
--

DROP TABLE IF EXISTS `game_hero_equip_doc`;
CREATE TABLE `game_hero_equip_doc` (
  `id` int UNSIGNED NOT NULL COMMENT '主键',
  `uid` int UNSIGNED NOT NULL COMMENT '用户ID',
  `equip_id` int UNSIGNED NOT NULL COMMENT '装备ID',
  `has_receive` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '是否领奖0否 1是',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='装备图鉴';

-- --------------------------------------------------------

--
-- 表的结构 `game_passport`
--

DROP TABLE IF EXISTS `game_passport`;
CREATE TABLE `game_passport` (
  `id` int UNSIGNED NOT NULL COMMENT 'passport_id',
  `name` varchar(64) NOT NULL COMMENT 'passport_name',
  `password` varchar(32) NOT NULL DEFAULT '',
  `login_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '登陆时间',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='账号表';

-- --------------------------------------------------------

--
-- 表的结构 `game_user`
--

DROP TABLE IF EXISTS `game_user`;
CREATE TABLE `game_user` (
  `id` int UNSIGNED NOT NULL COMMENT '用户ID',
  `name` varchar(32) NOT NULL COMMENT '用户名',
  `level` smallint UNSIGNED NOT NULL DEFAULT '1' COMMENT '等级',
  `avatar` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '头像',
  `ftue` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '新手引导步骤号',
  `main_hero_id` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '主将ID',
  `sub_hero_id` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '出战伙伴ID',
  `gold` bigint UNSIGNED NOT NULL DEFAULT '0' COMMENT '金币',
  `gold_flush_in` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '金币最新结算时间',
  `gold_buff_end_time` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '双倍金币结束时间',
  `diamond` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '钻石',
  `resource` blob NOT NULL COMMENT '其他资源',
  `attr` blob COMMENT '二级属性',
  `equips` blob NOT NULL COMMENT '装备信息',
  `campaign_num` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '通过的关卡数',
  `building_effect` blob NOT NULL COMMENT '建筑效果',
  `tech_effect` blob NOT NULL COMMENT '科技效果',
  `cast_effect` tinyblob NOT NULL COMMENT '锻造信息',
  `data_version` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '数据版本号',
  `extra` tinyblob NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='用户表';

-- --------------------------------------------------------

--
-- 表的结构 `game_user_annals`
--

DROP TABLE IF EXISTS `game_user_annals`;
CREATE TABLE `game_user_annals` (
  `id` int UNSIGNED NOT NULL COMMENT '用户ID',
  `done_list` blob NOT NULL COMMENT '已领取奖励',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='成就表';

-- --------------------------------------------------------

--
-- 表的结构 `game_user_arena`
--

DROP TABLE IF EXISTS `game_user_arena`;
CREATE TABLE `game_user_arena` (
  `id` int UNSIGNED NOT NULL COMMENT '唯一ID',
  `uid` int UNSIGNED NOT NULL COMMENT '用户ID',
  `is_player` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '是否是用户',
  `name` varchar(32) NOT NULL DEFAULT '' COMMENT '名称',
  `avatar` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '头像',
  `level` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '等级',
  `fighting_capacity` bigint UNSIGNED NOT NULL DEFAULT '0' COMMENT '战斗力',
  `sign_week` mediumint UNSIGNED NOT NULL DEFAULT '0' COMMENT '参加周',
  `group_id` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '分组id',
  `rank` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '排名',
  `send_reward` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '已经发奖',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='竞技场表';

-- --------------------------------------------------------

--
-- 表的结构 `game_user_building`
--

DROP TABLE IF EXISTS `game_user_building`;
CREATE TABLE `game_user_building` (
  `ID` int UNSIGNED NOT NULL COMMENT '主键',
  `uid` int UNSIGNED NOT NULL COMMENT '用户ID',
  `building_id` int UNSIGNED NOT NULL COMMENT '建筑ID',
  `level` smallint NOT NULL COMMENT '当前等级',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='用户建筑表';

-- --------------------------------------------------------

--
-- 表的结构 `game_user_crystal`
--

DROP TABLE IF EXISTS `game_user_crystal`;
CREATE TABLE `game_user_crystal` (
  `ID` int UNSIGNED NOT NULL COMMENT '主键',
  `uid` int UNSIGNED NOT NULL COMMENT '用户ID',
  `crystal_id` int UNSIGNED NOT NULL COMMENT '水晶ID',
  `level` smallint NOT NULL COMMENT '当前等级',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='用户水晶表';

-- --------------------------------------------------------

--
-- 表的结构 `game_user_explore`
--

DROP TABLE IF EXISTS `game_user_explore`;
CREATE TABLE `game_user_explore` (
  `id` int UNSIGNED NOT NULL COMMENT '主键',
  `uid` int UNSIGNED NOT NULL COMMENT '用户ID',
  `explore_id` tinyint UNSIGNED NOT NULL COMMENT '探索ID',
  `start_time` int UNSIGNED NOT NULL COMMENT '开始时间',
  `hero_ids` blob NOT NULL COMMENT '探索详情',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='用户探索表';

-- --------------------------------------------------------

--
-- 表的结构 `game_user_extend`
--

DROP TABLE IF EXISTS `game_user_extend`;
CREATE TABLE `game_user_extend` (
  `id` int UNSIGNED NOT NULL COMMENT '用户id',
  `campaign_id` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '关卡ID',
  `campaign_time` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '通关时间',
  `campaign_old_id` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '旧的关卡ID',
  `online_reward` tinyblob NOT NULL COMMENT '在线奖励数据',
  `apocalypse` tinyblob NOT NULL COMMENT '天启数据',
  `tower` blob NOT NULL COMMENT '怪物入侵数据',
  `arena_remain_num` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '竞技场剩余次数',
  `arena_sign_week` mediumint UNSIGNED NOT NULL DEFAULT '0' COMMENT '竞技场报名周',
  `alchemy` tinyblob COMMENT '炼药',
  `business_man` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '商人记录',
  `ads` tinyblob NOT NULL COMMENT '广告数据',
  `last_modify_day` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '最后重置日期Ymd',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='用户扩展信息表';

-- --------------------------------------------------------

--
-- 表的结构 `game_user_hero`
--

DROP TABLE IF EXISTS `game_user_hero`;
CREATE TABLE `game_user_hero` (
  `id` int UNSIGNED NOT NULL COMMENT '主键ID',
  `uid` int UNSIGNED NOT NULL COMMENT '用户ID',
  `hero_id` int UNSIGNED NOT NULL COMMENT '英雄ID',
  `type` tinyint UNSIGNED NOT NULL COMMENT '英雄类型1主角 2伙伴',
  `name` varchar(16) NOT NULL COMMENT '英雄名',
  `level` smallint UNSIGNED NOT NULL DEFAULT '1' COMMENT '等级',
  `evolve_times` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '突破次数',
  `fighting_capacity` int UNSIGNED NOT NULL DEFAULT '0' COMMENT '战斗力',
  `attack_freq` smallint UNSIGNED NOT NULL COMMENT '基础攻击频率',
  `attack_trans` smallint UNSIGNED NOT NULL COMMENT '攻击转换比例',
  `defend_trans` smallint UNSIGNED NOT NULL COMMENT '防御转换比例',
  `hp_trans` smallint UNSIGNED NOT NULL COMMENT '血量转化率',
  `attack_ratio` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '锻魂攻击加成',
  `defend_ratio` smallint UNSIGNED NOT NULL DEFAULT '0' COMMENT '锻魂防御加成',
  `sex` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '性别',
  `race` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '种族',
  `skills` tinyblob NOT NULL COMMENT '技能信息',
  `skin_id` int UNSIGNED NOT NULL COMMENT '皮肤ID',
  `skin_map` tinyblob NOT NULL COMMENT '皮肤数组',
  `explore_id` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '探索ID',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='英雄表';

-- --------------------------------------------------------

--
-- 表的结构 `game_user_mail`
--

DROP TABLE IF EXISTS `game_user_mail`;
CREATE TABLE `game_user_mail` (
  `id` int UNSIGNED NOT NULL COMMENT '主键',
  `uid` int UNSIGNED NOT NULL COMMENT '用户ID',
  `mail_id` int UNSIGNED NOT NULL COMMENT '邮件',
  `params` tinyblob COMMENT '参数',
  `status` tinyint UNSIGNED NOT NULL DEFAULT '0' COMMENT '状态0未读1已读2删除',
  `has_received` tinyint UNSIGNED DEFAULT '0' COMMENT '附件状态0未领取1已领取',
  `attachment` varchar(128) NOT NULL DEFAULT '' COMMENT '附件内容',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='用户邮件';

-- --------------------------------------------------------

--
-- 表的结构 `game_user_tech`
--

DROP TABLE IF EXISTS `game_user_tech`;
CREATE TABLE `game_user_tech` (
  `ID` int UNSIGNED NOT NULL COMMENT '主键',
  `uid` int UNSIGNED NOT NULL COMMENT '用户ID',
  `tech_id` int UNSIGNED NOT NULL COMMENT '科技ID',
  `level` smallint NOT NULL COMMENT '当前等级',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='用户科技表';

--
-- 转储表的索引
--

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
  MODIFY `id` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'ID';

--
-- 使用表AUTO_INCREMENT `game_hero_equip_doc`
--
ALTER TABLE `game_hero_equip_doc`
  MODIFY `id` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键';

--
-- 使用表AUTO_INCREMENT `game_passport`
--
ALTER TABLE `game_passport`
  MODIFY `id` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'passport_id';

--
-- 使用表AUTO_INCREMENT `game_user_arena`
--
ALTER TABLE `game_user_arena`
  MODIFY `id` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '唯一ID';

--
-- 使用表AUTO_INCREMENT `game_user_building`
--
ALTER TABLE `game_user_building`
  MODIFY `ID` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键';

--
-- 使用表AUTO_INCREMENT `game_user_crystal`
--
ALTER TABLE `game_user_crystal`
  MODIFY `ID` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键';

--
-- 使用表AUTO_INCREMENT `game_user_explore`
--
ALTER TABLE `game_user_explore`
  MODIFY `id` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键';

--
-- 使用表AUTO_INCREMENT `game_user_hero`
--
ALTER TABLE `game_user_hero`
  MODIFY `id` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键ID';

--
-- 使用表AUTO_INCREMENT `game_user_mail`
--
ALTER TABLE `game_user_mail`
  MODIFY `id` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键';

--
-- 使用表AUTO_INCREMENT `game_user_tech`
--
ALTER TABLE `game_user_tech`
  MODIFY `ID` int UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '主键';
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
