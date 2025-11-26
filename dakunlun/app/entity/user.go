package entity

import (
	"dakunlun/app/util"
	"database/sql/driver"
	"fmt"

	"encoding/json"
)

const (
	InitUserLevel  = 1
	InitGold       = 50000
	InitDiamond    = 500
	InitSecondAttr = 1000
	ChangeNameCost = 1000  //钻石
	InitAvatarID   = 1     //初始头像
	GoldBuffMin    = 36000 //buff时间小于10小时才可继续看图腾广告
	GoldBuffIncr   = 21600 //图腾每次增加buff 6小时
)

const (
	EffectIDCritical    = 20001
	EffectIDTenacity    = 20002
	EffectIDBreak       = 20003
	EffectIDImpregnable = 20004
	EffectIDHit         = 20005
	EffectIDDodge       = 20006
	EffectIDDefuse      = 20007
)

type UserEntity struct {
	Model
	Name            string   `gorm:"type:VARCHAR(32);not null;default:'';comment:用户名"`
	Avatar          uint16   `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:1;comment:头像ID"`
	Level           uint16   `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:1;comment:等级"`
	Ftue            uint8    `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:引导步骤"`
	MainHeroID      uint32   `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	SubHeroID       uint32   `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:伙伴ID"`
	Gold            uint64   `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:金币数"`
	GoldFlushIn     int64    `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:刷新时间"`
	GoldBuffEndTime int64    `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:到期时间"`
	Diamond         uint32   `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:钻石数"`
	Resource        Resource `gorm:"type:tinyblob"`
	Attr            Attr     `gorm:"type:blob"`
	Equips          Equips   `gorm:"type:blob"`
	CampaignNum     uint32   `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:关卡ID"`
	BuildingEffect  BEffect  `gorm:"type:tinyblob"`
	TechEffect      TEffect  `gorm:"type:blob"`
	CastEffect      CEffect  `gorm:"type:tinyblob"`
	DataVersion     uint16   `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:关卡ID"`
	Extra           Extra    `gorm:"type:tinyblob"`
}

type Resource struct {
	//魂石
	SoulCrystal uint32
	//宝物精华
	TreasureAnima uint32
	//狂妄之章
	SuperbiaStone uint32
	//憎恶之章
	InvidiaStone uint32
	//懈怠之章
	AcediaStone uint32
	//吞噬之章
	GulaStone uint32
	//无厌之章
	AvaritiaStone uint32
	//销魂之章
	LuxuriaStone uint32
	//肆虐之章
	IraStone uint32
	//本源之力
	BenYuan uint32
	//潜能之力
	QianNeng uint32
	//元素结晶
	Element uint32
	//秘传书
	Book uint32
}

func (r *Resource) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Resource value:", value)
	}
	result := Resource{}
	if len(bytes) > 0 {
		err := json.Unmarshal(bytes, &result)
		if err != nil {
			return err
		}
	}

	*r = result

	return nil
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r Resource) Value() (driver.Value, error) {
	return json.Marshal(r)
}

type BEffect struct {
	//金币基础产量
	GoldMill       uint32
	GoldTavern     uint32
	GoldMine       uint32
	GoldMetallurgy uint32

	//金币加成总
	GoldRatio uint32
}

func (be *BEffect) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal BEffect value:", value)
	}
	result := BEffect{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*be = result
	return err
}

func (be BEffect) Value() (driver.Value, error) {
	return json.Marshal(be)
}

type TEffect struct {
	//金币产量加成
	MillRatio       uint32
	TavernRatio     uint32
	MineRatio       uint32
	MetallurgyRatio uint32
	//主角升级折扣3
	MainHeroUpgradeRatio uint32
	//伙伴升级折扣7
	SubHeroUpgradeRatio uint32
	//主角战斗力4
	MainHeroFightingCapacityPlus uint64
	//伙伴战斗力8
	SubHeroFightingCapacityPlus uint64
	//战斗力5
	AttackTrans uint32
	//防御力6
	DefendTrans uint32
}

func (be *TEffect) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal TEffect value:", value)
	}
	result := TEffect{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*be = result
	return err
}

func (be TEffect) Value() (driver.Value, error) {
	return json.Marshal(be)
}

type Attr struct {
	//人属性
	Hit         uint32 //命中 影响角色攻击的命中能力
	Dodge       uint32 //闪避 影响角色闪避攻击的能力
	Critical    uint32 //暴击 影响角色的暴击几率
	Tenacity    uint32 //韧性 影响角色的被暴击几率
	Break       uint32 //破甲 无视对方一定防御能力
	Impregnable uint32 //铁壁 抵消破甲的效果
	Defuse      uint32 //化解 影响角色最终减伤
	//装备加成
	HitEquipPlus              uint32 //命中 影响角色攻击的命中能力
	DodgeEquipPlus            uint32 //闪避 影响角色闪避攻击的能力
	CriticalEquipPlus         uint32 //暴击 影响角色的暴击几率
	TenacityEquipPlus         uint32 //韧性 影响角色的被暴击几率
	BreakEquipPlus            uint32 //破甲 无视对方一定防御能力
	ImpregnableEquipPlus      uint32 //铁壁 抵消破甲的效果
	DefuseEquipPlus           uint32 //化解 影响角色最终减伤
	FightingCapacityEquipPlus uint64 //战斗力 装备加成
	//水晶加成
	HitCrystalPlus         uint32 //命中 影响角色攻击的命中能力
	DodgeCrystalPlus       uint32 //闪避 影响角色闪避攻击的能力
	CriticalCrystalPlus    uint32 //暴击 影响角色的暴击几率
	TenacityCrystalPlus    uint32 //韧性 影响角色的被暴击几率
	BreakCrystalPlus       uint32 //破甲 无视对方一定防御能力
	ImpregnableCrystalPlus uint32 //铁壁 抵消破甲的效果
	DefuseCrystalPlus      uint32 //化解 影响角色最终减伤
}

func (r *Attr) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Attr value:", value)
	}
	result := Attr{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r Attr) Value() (driver.Value, error) {
	return json.Marshal(r)
}

type Equip struct {
	ID      uint32
	EquipID uint32
	SkillID uint32
	Effect1 int
	Effect2 int
	Effect3 int
}

type Equips map[uint8]Equip

func (r *Equips) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Skills value:", value)
	}
	result := Equips{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r Equips) Value() (driver.Value, error) {
	return json.Marshal(r)
}

type CEffect struct {
	//castid
	CastID uint32
	//角色战斗力增加
	FightingCapacityPlus uint64
}

func (ce *CEffect) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal CEffect value:", value)
	}
	result := CEffect{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*ce = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (ce CEffect) Value() (driver.Value, error) {
	return json.Marshal(ce)
}

type Extra struct {
	AccountType int
	// 下次刷新时间
	RefreshTime int64
	// 当月金额
	Total uint64
	// 单次限制
	SingleLimit uint64
	// 每月限制
	MonthLimit uint64
}

func (ex *Extra) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Extra value:", value)
	}
	result := Extra{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*ex = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (ex Extra) Value() (driver.Value, error) {
	return json.Marshal(ex)
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (ex Extra) Flush() {
	endOfMonth := util.Carbon().Now().EndOfMonth().ToTimestamp()
	if endOfMonth > ex.RefreshTime {
		ex.RefreshTime = endOfMonth
		ex.Total = 0
	}
}

// 设置表名，默认是结构体的名的复数形式
func (*UserEntity) TableName() string {
	return "game_user"
}

func NewUser(id uint32, accountType int) *UserEntity {
	var singleLimit, monthLimit uint64
	if accountType == 2 {
		singleLimit = 50
		monthLimit = 200
	}

	if accountType == 3 {
		singleLimit = 100
		monthLimit = 400
	}

	return &UserEntity{
		Avatar:   InitAvatarID,
		Level:    InitUserLevel,
		Gold:     InitGold,
		Diamond:  InitDiamond,
		Resource: Resource{},
		Attr: Attr{
			Hit:         InitSecondAttr,
			Dodge:       InitSecondAttr,
			Critical:    InitSecondAttr,
			Tenacity:    InitSecondAttr,
			Break:       InitSecondAttr,
			Impregnable: InitSecondAttr,
			Defuse:      InitSecondAttr,
		},
		Equips: Equips{
			PosOne:   Equip{},
			PosTwo:   Equip{},
			PosThree: Equip{},
			PosFour:  Equip{},
			PosFive:  Equip{},
			PosSix:   Equip{},
		},
		CampaignNum:     0,
		BuildingEffect:  BEffect{},
		TechEffect:      TEffect{},
		GoldFlushIn:     util.Carbon().Now().ToTimestamp(),
		GoldBuffEndTime: 0,
		MainHeroID:      0,
		SubHeroID:       0,
		CastEffect:      CEffect{},
		Model: Model{
			ID: id,
		},
		DataVersion: 0,
		Extra: Extra{
			AccountType: accountType,
			MonthLimit:  monthLimit,
			SingleLimit: singleLimit,
			RefreshTime: util.Carbon().Now().EndOfMonth().ToTimestamp(),
		},
	}
}

func (entity *UserEntity) HidingUid() uint32 {
	return util.EncodeID(entity.ID)
}

// 同步金币
func (entity *UserEntity) CurGold() uint64 {
	now := util.Carbon().Now().ToTimestamp()
	// 需要结算双倍经验
	if entity.GoldBuffEndTime > entity.GoldFlushIn {
		if now > entity.GoldBuffEndTime {
			entity.Gold += 2 * (entity.GoldIncrement() * uint64(entity.GoldBuffEndTime-entity.GoldFlushIn))
			entity.GoldFlushIn = entity.GoldBuffEndTime
		} else {
			entity.Gold += 2 * (entity.GoldIncrement() * uint64(now-entity.GoldFlushIn))
			entity.GoldFlushIn = now
		}
	}

	if now > entity.GoldFlushIn {
		diffSec := uint64(now - entity.GoldFlushIn)
		entity.Gold += (entity.GoldIncrement() * diffSec)
		entity.GoldFlushIn = now
	}
	return entity.Gold
}

// 增加双倍金币buff
func (entity *UserEntity) IncrGoldBuff(s int64) error {
	entity.CurGold()
	now := util.Carbon().Now().ToTimestamp()
	//强制同步下金币结算时间防止跨秒
	entity.GoldFlushIn = now

	//剩余时间小于10小时 则可用
	if entity.GoldBuffEndTime-entity.GoldFlushIn >= GoldBuffMin {
		return util.NewAppError(util.ErrorCodeTimeError2)
	}

	if entity.GoldBuffEndTime >= entity.GoldFlushIn {
		entity.GoldBuffEndTime += s
	} else {
		entity.GoldBuffEndTime = now + s
	}

	return nil
}

// 使用中的装备ID列表
func (entity *UserEntity) EquipIDs() (r []uint32) {
	for _, v := range entity.Equips {
		if v.ID > 0 {
			r = append(r, v.ID)
		}
	}
	return
}

// 使用中的装备ID列表
func (entity *UserEntity) SetEquipPlus(effectID, effectValue uint32) {
	switch effectID {
	case EffectIDHit:
		entity.Attr.HitEquipPlus += effectValue
	case EffectIDDodge:
		entity.Attr.DodgeEquipPlus += effectValue
	case EffectIDCritical:
		entity.Attr.CriticalEquipPlus += effectValue
	case EffectIDTenacity:
		entity.Attr.TenacityEquipPlus += effectValue
	case EffectIDBreak:
		entity.Attr.BreakEquipPlus += effectValue
	case EffectIDImpregnable:
		entity.Attr.ImpregnableEquipPlus += effectValue
	case EffectIDDefuse:
		entity.Attr.DefuseEquipPlus += effectValue
	}
}

// 使用中的装备ID列表
func (entity *UserEntity) ClearEquipPlus() {
	entity.Attr.HitEquipPlus = 0
	entity.Attr.DodgeEquipPlus = 0
	entity.Attr.CriticalEquipPlus = 0
	entity.Attr.TenacityEquipPlus = 0
	entity.Attr.BreakEquipPlus = 0
	entity.Attr.ImpregnableEquipPlus = 0
	entity.Attr.DefuseEquipPlus = 0
}

func (entity *UserEntity) GetAttrTotal() uint32 {
	return entity.GetHit() + entity.GetDodge() + entity.GetCritical() + entity.GetTenacity() + entity.GetBreak() + entity.GetDefuse() + entity.GetHit()
}

func (entity *UserEntity) GetHit() uint32 {
	return entity.Attr.Hit + entity.Attr.HitEquipPlus + entity.Attr.HitCrystalPlus
}
func (entity *UserEntity) GetDodge() uint32 {
	return entity.Attr.Dodge + entity.Attr.DodgeEquipPlus + entity.Attr.DodgeCrystalPlus
}
func (entity *UserEntity) GetCritical() uint32 {
	return entity.Attr.Critical + entity.Attr.CriticalEquipPlus + entity.Attr.CriticalCrystalPlus
}

func (entity *UserEntity) GetTenacity() uint32 {
	return entity.Attr.Tenacity + entity.Attr.TenacityEquipPlus + entity.Attr.TenacityCrystalPlus
}
func (entity *UserEntity) GetBreak() uint32 {
	return entity.Attr.Break + entity.Attr.BreakEquipPlus + entity.Attr.BreakCrystalPlus
}
func (entity *UserEntity) GetImpregnable() uint32 {
	return entity.Attr.Impregnable + entity.Attr.ImpregnableEquipPlus + entity.Attr.ImpregnableCrystalPlus
}
func (entity *UserEntity) GetDefuse() uint32 {
	return entity.Attr.Defuse + entity.Attr.DefuseEquipPlus + entity.Attr.DefuseCrystalPlus
}

func (entity *UserEntity) GoldIncrement() uint64 {
	return uint64((entity.GetGoldMill() + entity.GetGoldTavern() + entity.GetGoldMine() + entity.GetGoldMetallurgy()) * (1 + float64(entity.BuildingEffect.GoldRatio)/BaseMultiple))
}

func (entity *UserEntity) GetGoldMill() float64 {
	return float64(entity.BuildingEffect.GoldMill) * (1 + float64(entity.TechEffect.MillRatio)/BaseMultiple)
}

func (entity *UserEntity) GetGoldTavern() float64 {
	return float64(entity.BuildingEffect.GoldTavern) * (1 + float64(entity.TechEffect.TavernRatio)/BaseMultiple)
}

func (entity *UserEntity) GetGoldMine() float64 {
	return float64(entity.BuildingEffect.GoldMine) * (1 + float64(entity.TechEffect.MineRatio)/BaseMultiple)
}

func (entity *UserEntity) GetGoldMetallurgy() float64 {
	return float64(entity.BuildingEffect.GoldMetallurgy) * (1 + float64(entity.TechEffect.MetallurgyRatio)/BaseMultiple)
}

func (entity *UserEntity) GetName(mustHave bool) string {
	if entity.Name != "" {
		return entity.Name
	}

	if mustHave {
		return fmt.Sprintf("游客%d", entity.HidingUid())
	}

	return ""
}
