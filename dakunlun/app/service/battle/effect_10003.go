package battle

type Effect10003 struct {
	*EffectBase
}

func newEffect10003(Skill *Skill) *Effect10003 {
	return &Effect10003{
		normalEffect(Skill),
	}
}

func (e *Effect10003) execute(b *BattleInfo) bool {
	return true
}
