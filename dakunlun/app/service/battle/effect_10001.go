package battle

type Effect10001 struct {
	*EffectBase
}

func newEffect10001(Skill *Skill) *Effect10001 {
	return &Effect10001{
		normalEffect(Skill),
	}
}

func (e *Effect10001) execute(b *BattleInfo) bool {
	return true
}
