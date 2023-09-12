<div align=center>

# 𝕳𝖆𝖑𝖑𝖔𝖜𝖊𝖊𝖓, 𝕲𝖍𝖔𝖘𝖙 𝕸𝖎𝖋𝖋𝖞

</div>


# ⚠️ 작업 전 아래 내용을 꼭 숙지해주세요
- [커밋 룰](#커밋-룰)


# 📝다른 문서도 확인해보세요
- [다이얼로그 시스템](/Documents/Dialogue-System.md)


## 커밋 룰

### 커밋 메세지 구조

- 기본적으로 커밋 메시지는 아래와 같이 제목/본문/꼬리말로 구성한다.

```
Type: Subject

body

footer
```

### 커밋 종류

```
- feat 		: 새로운 기능 추가
- fix 		: 버그 수정
- docs 		: 문서 수정
- style 	: 코드 formatting, 세미콜론(;) 누락, 코드 변경이 없는 경우
- refactor 	: 코드 리팩토링
- test 		: 테스트 코드, 리팽토링 테스트 코드 추가
- chore 	: 빌드 업무 수정, 패키지 매니저 수정
```

### 제목

- 제목은 50자를 넘기지 않고, 마침표를 붙이지 않는다.
- 제목에는 위 커밋 종류를 함께 쓴다.
- 과거시제를 사용하지 않고 명령조로 작성한다.
- 제목과 본문은 한 줄 띄워 분리한다.
- 제목의 첫 글자는 반드시 대문자로 쓴다.
- 제목이나 본문에 이슈 번호(가 있다면) 붙여야 한다.

### 본문

- 선택사항이기에 모든 커밋에 본문 내용을 작성할 필요는 없다.
- 한 줄에 72자를 넘기면 안된다.
- 어떻게(How)보다 무엇을, 왜(What, Why)에 맞춰 작성한다.
- 설명뿐만 아니라, 커밋의 이유를 작성할 때에도 쓴다.

### Footer

- 선택사항이기에 모든 커밋에 꼬릿말을 작성할 필요는 없다.
- Issue Tracker ID를 작성할 때 사용한다.

> 예시
> ```
> Resolves: #123
> See also: #456, #789
> ```

### 예시

- 예시는 영문이지만, 한글로 작성하셔도 무방합니다.

```
feat: Summarize changes in around 50 characters or less

More detailed explanatory text, if necessary. Wrap it to about 72
characters or so. In some contexts, the first line is treated as the
subject of the commit and the rest of the text as the body. The
blank line separating the summary from the body is critical (unless
you omit the body entirely); various tools like `log`, `shortlog`
and `rebase` can get confused if you run the two together.

Explain the problem that this commit is solving. Focus on why you
are making this change as opposed to how (the code explains that).
Are there side effects or other unintuitive consequences of this
change? Here's the place to explain them.

Further paragraphs come after blank lines.

 - Bullet points are okay, too

 - Typically a hyphen or asterisk is used for the bullet, preceded
   by a single space, with blank lines in between, but conventions
   vary here

If you use an issue tracker, put references to them at the bottom,
like this:

Resolves: #123
See also: #456, #789
```