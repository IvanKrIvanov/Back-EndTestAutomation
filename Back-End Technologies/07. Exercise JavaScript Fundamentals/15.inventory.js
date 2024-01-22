function createHeroRegister(heroData) {
    const heroes = [];
  
    for (const data of heroData) {
      const [name, level, items] = data.split(' / ');
      const hero = {
        name: name.trim(),
        level: Number(level.trim()),
        items: items ? items.split(', ').map(item => item.trim()) : []
      };
      heroes.push(hero);
    }
  

    heroes.sort((a, b) => a.level - b.level);
  

    const result = heroes.map(hero => (
      `Hero: ${hero.name}\nlevel => ${hero.level}\nitems => ${hero.items.join(', ')}\n`
    )).join('');
  
    console.log(result);
  }
  

  