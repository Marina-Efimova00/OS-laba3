using System;
using System.Collections.Generic;
using System.Text;

namespace laba3
{
    public class MemoryManagment
    {
        private Process process;
        private List<Page> clock;
        public MemoryManagment()
        {
            process = new Process(5, 15);
            this.clock = new List<Page>();
        }
        public int addPage(Process process)
        {
            int pageId = this.process.addPage(new Page(process.getId()));
            process.getPagesIds().Add(pageId);
            return pageId;
        }
        public Page getPage(int pageId)
        {
            Page page = this.process.getPage(pageId);
            if (page.isPresent())
            {
                page.setRecourse(true);
            }
            else
            {
                int emptyPageId = OperatingSystem.memory.getEmptyPageId();
                if (emptyPageId != -1)
                {
                    OperatingSystem.memory.setPage(emptyPageId, page);
                    page.setRecourse(true);
                    page.setPresence(true);
                    page.setPhysicalAddress(emptyPageId);
                    this.clock.Add(page);
                }
                else
                {
                    while (true)
                    {
                        Page replacePage = this.clock[0];
                        clock.RemoveAt(clock.Count - 1);
                        if (replacePage.isRecourse())
                        {
                            replacePage.setRecourse(false);
                            this.clock.Add(replacePage);
                        }
                        else
                        {
                            if (replacePage.getVirtualAddress() != -1)
                            {
                                OperatingSystem.memory.setPage(replacePage.getPhysicalAddress(),
                                      OperatingSystem.returnPage(replacePage.getVirtualAddress()));
                            }
                            else
                            {
                                OperatingSystem.memory.setPage(replacePage.getPhysicalAddress(), page);
                            }
                            page.setRecourse(true);
                            page.setPresence(true);
                            page.setPhysicalAddress(replacePage.getPhysicalAddress());
                            this.clock.Add(page);
                            replacePage.setPresence(false);
                            replacePage.setVirtualAddress(process.addPage(replacePage));
                            replacePage.setPhysicalAddress(-1);
                            break;
                        }
                    }
                }
            }
            return page;
        }
    }
}
