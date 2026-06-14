import { mkdir, writeFile, rm } from 'node:fs/promises';
import path from 'node:path';

const BASE_URL = 'https://docs.atas.net/en/';
const OUTPUT_DIR = path.resolve('docs/atas');
const INTERNAL_HOST = 'docs.atas.net';

const decodeEntities = (input) => input
  .replace(/&#(\d+);/g, (_, code) => String.fromCharCode(Number(code)))
  .replace(/&#x([0-9a-fA-F]+);/g, (_, code) => String.fromCharCode(parseInt(code, 16)))
  .replace(/&nbsp;/g, ' ')
  .replace(/&amp;/g, '&')
  .replace(/&lt;/g, '<')
  .replace(/&gt;/g, '>')
  .replace(/&quot;/g, '"')
  .replace(/&#39;/g, "'");

const stripTags = (input) => decodeEntities(input.replace(/<[^>]+>/g, ''));

const normalizeWhitespace = (text) => text
  .replace(/\r/g, '')
  .replace(/\t/g, ' ')
  .replace(/[ \u00a0]+\n/g, '\n')
  .replace(/\n{3,}/g, '\n\n')
  .replace(/[ ]{2,}/g, ' ')
  .trim();

const tableToMarkdown = (tableHtml) => {
  const rows = [...tableHtml.matchAll(/<tr[^>]*>([\s\S]*?)<\/tr>/gi)]
    .map((match) => [...match[1].matchAll(/<t[dh][^>]*>([\s\S]*?)<\/t[dh]>/gi)]
      .map((cell) => normalizeWhitespace(stripTags(cell[1]))));

  if (!rows.length) return '';

  const width = Math.max(...rows.map((row) => row.length));
  const normalizedRows = rows.map((row) => {
    const copy = [...row];
    while (copy.length < width) copy.push('');
    return copy;
  });

  const header = normalizedRows[0];
  const separator = new Array(width).fill('---');
  const body = normalizedRows.slice(1);
  return [header, separator, ...body]
    .map((row) => `| ${row.join(' | ')} |`)
    .join('\n');
};

const getCategory = (page) => {
  if (page === 'index.html') return 'guides';
  if (page === 'README_8md.html') return 'api/overviews';
  if (page.startsWith('md_')) return 'guides';
  if (page.includes('-members.html')) return 'api/members';
  if (page.startsWith('class')) return 'api/classes';
  if (page.startsWith('interface')) return 'api/interfaces';
  if (page.startsWith('namespace')) return 'api/namespaces';
  if (page.startsWith('struct')) return 'api/structs';
  if (page.startsWith('dir_')) return 'api/directories';
  if (/_8cs\.html$|_8g\.html$/.test(page)) return 'api/files';
  return 'api/overviews';
};

const pageToOutputPath = (page) => path.posix.join(getCategory(page), page.replace(/\.html$/, '.md'));

const relativeDocLink = (fromOutputPath, toPage, anchor = '') => {
  const target = pageToOutputPath(toPage);
  const fromDir = path.posix.dirname(fromOutputPath);
  const rel = path.posix.relative(fromDir, target) || path.posix.basename(target);
  return `${rel.startsWith('.') ? rel : `./${rel}`}${anchor}`;
};

const slugToMarkdownPath = (href, currentOutputPath) => {
  if (!href) return href;
  if (href.startsWith('mailto:') || href.startsWith('tel:')) return href;

  let anchor = '';
  const anchorIndex = href.indexOf('#');
  if (anchorIndex >= 0) {
    anchor = href.slice(anchorIndex);
    href = href.slice(0, anchorIndex);
  }

  try {
    const url = new URL(href, BASE_URL);
    if (url.host !== INTERNAL_HOST) return url.toString();
    const fileName = path.posix.basename(url.pathname);
    if (!fileName.endsWith('.html')) return url.toString();
    return relativeDocLink(currentOutputPath, fileName, anchor);
  } catch {
    if (href.endsWith('.html')) return relativeDocLink(currentOutputPath, href, anchor);
    return href + anchor;
  }
};

const htmlToMarkdown = (html, currentOutputPath) => {
  let text = html;

  text = text.replace(/<script[\s\S]*?<\/script>/gi, '');
  text = text.replace(/<style[\s\S]*?<\/style>/gi, '');
  text = text.replace(/<img[^>]*alt="([^"]*)"[^>]*>/gi, '$1');
  text = text.replace(/<img[^>]*>/gi, '');

  text = text.replace(/<a[^>]*href="([^"]+)"[^>]*>([\s\S]*?)<\/a>/gi, (_, href, inner) => {
    const label = normalizeWhitespace(stripTags(inner)) || href;
    return `[${label}](${slugToMarkdownPath(href, currentOutputPath)})`;
  });

  text = text.replace(/<pre[^>]*>([\s\S]*?)<\/pre>/gi, (_, code) => {
    const cleaned = decodeEntities(code).replace(/<[^>]+>/g, '').trimEnd();
    return `\n\n\`\`\`\n${cleaned}\n\`\`\`\n\n`;
  });

  text = text.replace(/<code[^>]*>([\s\S]*?)<\/code>/gi, (_, code) => `\`${normalizeWhitespace(decodeEntities(code))}\``);
  text = text.replace(/<table[^>]*>([\s\S]*?)<\/table>/gi, (_, table) => `\n\n${tableToMarkdown(table)}\n\n`);

  for (let level = 6; level >= 1; level -= 1) {
    const pattern = new RegExp(`<h${level}[^>]*>([\\s\\S]*?)<\\/h${level}>`, 'gi');
    text = text.replace(pattern, (_, inner) => {
      const heading = normalizeWhitespace(stripTags(inner));
      return heading ? `\n\n${'#'.repeat(level)} ${heading}\n\n` : '';
    });
  }

  text = text.replace(/<li[^>]*>([\s\S]*?)<\/li>/gi, (_, inner) => {
    const item = normalizeWhitespace(stripTags(inner));
    return item ? `- ${item}\n` : '';
  });

  text = text.replace(/<br\s*\/?>/gi, '\n');
  text = text.replace(/<\/p>/gi, '\n\n');
  text = text.replace(/<\/div>/gi, '\n\n');
  text = text.replace(/<\/ul>/gi, '\n');
  text = text.replace(/<\/ol>/gi, '\n');
  text = text.replace(/<[^>]+>/g, '');

  text = decodeEntities(text);
  text = text.replace(/[ \t]+\n/g, '\n');
  text = text.replace(/\n{3,}/g, '\n\n');
  return normalizeWhitespace(text);
};

const extractTitle = (html, fallbackSlug) => {
  const titleMatch = html.match(/<div class="title">([\s\S]*?)<\/div>/i)
    || html.match(/<title>([\s\S]*?)<\/title>/i);
  const title = titleMatch ? normalizeWhitespace(stripTags(titleMatch[1])) : fallbackSlug;
  return title.replace(/^ATAS:\s*/i, '').trim();
};

const extractContent = (html) => {
  const contentsStart = html.indexOf('<div class="contents">');
  if (contentsStart >= 0) {
    const afterStart = html.slice(contentsStart);
    const endMarker = afterStart.indexOf('<!-- contents -->');
    if (endMarker >= 0) return afterStart.slice(0, endMarker);
    const docEnd = afterStart.indexOf('</div><!-- PageDoc -->');
    if (docEnd >= 0) return afterStart.slice(0, docEnd);
    return afterStart;
  }

  const bodyMatch = html.match(/<body[^>]*>([\s\S]*?)<\/body>/i);
  return bodyMatch ? bodyMatch[1] : html;
};

const getInternalLinks = (html) => {
  const links = new Set();
  for (const match of html.matchAll(/href="([^"]+)"/gi)) {
    const href = match[1];
    try {
      const url = new URL(href, BASE_URL);
      if (url.host === INTERNAL_HOST && url.pathname.startsWith('/en/') && url.pathname.endsWith('.html')) {
        links.add(path.posix.basename(url.pathname));
      }
    } catch {
      if (/^[^#?]+\.html(?:#.*)?$/.test(href)) {
        links.add(href.split('#')[0]);
      }
    }
  }
  return [...links];
};

const fetchText = async (url) => {
  const response = await fetch(url);
  if (!response.ok) throw new Error(`Failed ${response.status} for ${url}`);
  return response.text();
};

const getSeedPages = async () => {
  const navtree = await fetchText(new URL('navtreedata.js', BASE_URL));
  const pages = new Set(['index.html']);
  for (const match of navtree.matchAll(/"([^"]+\.html)"/g)) pages.add(match[1]);
  return [...pages];
};

const ensureCleanDir = async () => {
  await rm(OUTPUT_DIR, { recursive: true, force: true });
  await mkdir(OUTPUT_DIR, { recursive: true });
};

const writePage = async ({ page, title, html }) => {
  const outputPath = pageToOutputPath(page);
  const fullPath = path.join(OUTPUT_DIR, outputPath);
  await mkdir(path.dirname(fullPath), { recursive: true });

  const markdownBody = htmlToMarkdown(extractContent(html), outputPath);
  const url = new URL(page, BASE_URL).toString();
  const markdown = [`# ${title}`, '', `Source: ${url}`, '', markdownBody].join('\n');
  await writeFile(fullPath, `${markdown.trim()}\n`, 'utf8');

  return { page, title, outputPath, url };
};

const buildSectionReadme = async (title, folder, pages) => {
  const lines = [`# ${title}`, ''];
  for (const page of pages) {
    lines.push(`- [${page.title}](./${path.posix.basename(page.outputPath)})`);
  }
  lines.push('');
  const target = path.join(OUTPUT_DIR, folder, 'README.md');
  await mkdir(path.dirname(target), { recursive: true });
  await writeFile(target, lines.join('\n'), 'utf8');
};

const main = async () => {
  await ensureCleanDir();

  const queue = await getSeedPages();
  const visited = new Set();
  const rawPages = [];

  while (queue.length) {
    const page = queue.shift();
    if (visited.has(page)) continue;
    visited.add(page);

    const html = await fetchText(new URL(page, BASE_URL));
    rawPages.push({ page, html, title: extractTitle(html, page.replace(/\.html$/, '')) });

    for (const link of getInternalLinks(html)) {
      if (!visited.has(link)) queue.push(link);
    }
  }

  const pages = [];
  for (const rawPage of rawPages) {
    pages.push(await writePage(rawPage));
  }

  pages.sort((a, b) => a.outputPath.localeCompare(b.outputPath));

  const sections = new Map();
  for (const page of pages) {
    const folder = path.posix.dirname(page.outputPath);
    if (!sections.has(folder)) sections.set(folder, []);
    sections.get(folder).push(page);
  }

  for (const [folder, items] of sections.entries()) {
    items.sort((a, b) => a.title.localeCompare(b.title));
    const sectionTitle = folder.split('/').map((part) => part[0].toUpperCase() + part.slice(1)).join(' / ');
    await buildSectionReadme(sectionTitle, folder, items);
  }

  const rootLines = [
    '# ATAS Documentation',
    '',
    `Source root: ${BASE_URL}`,
    `Generated: ${new Date().toISOString()}`,
    `Pages: ${pages.length}`,
    '',
    '## Sections',
    '',
    '- [Guides](./guides/README.md)',
    '- [API Classes](./api/classes/README.md)',
    '- [API Interfaces](./api/interfaces/README.md)',
    '- [API Members](./api/members/README.md)',
    '- [API Namespaces](./api/namespaces/README.md)',
    '- [API Structs](./api/structs/README.md)',
    '- [API Files](./api/files/README.md)',
    '- [API Directories](./api/directories/README.md)',
    '- [API Overviews](./api/overviews/README.md)',
    ''
  ];

  await writeFile(path.join(OUTPUT_DIR, 'README.md'), rootLines.join('\n'), 'utf8');
  console.log(`Saved ${pages.length} pages to ${OUTPUT_DIR}`);
};

main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
